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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class FacetModel : IComparable
	{
        public string FacetModelId { set; get; }
        public string Field { set; get; }
        public string Type { set; get; }
        public string DisplayName { set; get; }
        public int? Order { set; get; }
        public FacetModel WithFacetModelId(string facetModelId) {
            this.FacetModelId = facetModelId;
            return this;
        }
        public FacetModel WithField(string field) {
            this.Field = field;
            return this;
        }
        public FacetModel WithType(string type) {
            this.Type = type;
            return this;
        }
        public FacetModel WithDisplayName(string displayName) {
            this.DisplayName = displayName;
            return this;
        }
        public FacetModel WithOrder(int? order) {
            this.Order = order;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):model:facet:(?<field>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):model:facet:(?<field>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):model:facet:(?<field>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _fieldRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+):model:facet:(?<field>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetFieldFromGrn(
            string grn
        )
        {
            var match = _fieldRegex.Match(grn);
            if (!match.Success || !match.Groups["field"].Success)
            {
                return null;
            }
            return match.Groups["field"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static FacetModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new FacetModel()
                .WithFacetModelId(!data.Keys.Contains("facetModelId") || data["facetModelId"] == null ? null : data["facetModelId"].ToString())
                .WithField(!data.Keys.Contains("field") || data["field"] == null ? null : data["field"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithDisplayName(!data.Keys.Contains("displayName") || data["displayName"] == null ? null : data["displayName"].ToString())
                .WithOrder(!data.Keys.Contains("order") || data["order"] == null ? null : (int?)(data["order"].ToString().Contains(".") ? (int)double.Parse(data["order"].ToString()) : int.Parse(data["order"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["facetModelId"] = FacetModelId,
                ["field"] = Field,
                ["type"] = Type,
                ["displayName"] = DisplayName,
                ["order"] = Order,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (FacetModelId != null) {
                writer.WritePropertyName("facetModelId");
                writer.Write(FacetModelId.ToString());
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

        public int CompareTo(object obj)
        {
            var other = obj as FacetModel;
            var diff = 0;
            if (FacetModelId == null && FacetModelId == other.FacetModelId)
            {
                // null and null
            }
            else
            {
                diff += FacetModelId.CompareTo(other.FacetModelId);
            }
            if (Field == null && Field == other.Field)
            {
                // null and null
            }
            else
            {
                diff += Field.CompareTo(other.Field);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (DisplayName == null && DisplayName == other.DisplayName)
            {
                // null and null
            }
            else
            {
                diff += DisplayName.CompareTo(other.DisplayName);
            }
            if (Order == null && Order == other.Order)
            {
                // null and null
            }
            else
            {
                diff += (int)(Order - other.Order);
            }
            return diff;
        }

        public void Validate() {
            {
                if (FacetModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facetModel", "log.facetModel.facetModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Field.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facetModel", "log.facetModel.field.error.tooLong"),
                    });
                }
            }
            {
                switch (Type) {
                    case "string":
                    case "double":
                    case "measure":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("facetModel", "log.facetModel.type.error.invalid"),
                        });
                }
            }
            {
                if (DisplayName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facetModel", "log.facetModel.displayName.error.tooLong"),
                    });
                }
            }
            {
                if (Order < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facetModel", "log.facetModel.order.error.invalid"),
                    });
                }
                if (Order > 100000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("facetModel", "log.facetModel.order.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new FacetModel {
                FacetModelId = FacetModelId,
                Field = Field,
                Type = Type,
                DisplayName = DisplayName,
                Order = Order,
            };
        }
    }
}