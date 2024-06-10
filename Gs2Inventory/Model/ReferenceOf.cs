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

namespace Gs2.Gs2Inventory.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ReferenceOf : IComparable
	{
        public string ReferenceOfId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public ReferenceOf WithReferenceOfId(string referenceOfId) {
            this.ReferenceOfId = referenceOfId;
            return this;
        }
        public ReferenceOf WithName(string name) {
            this.Name = name;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _inventoryNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetInventoryNameFromGrn(
            string grn
        )
        {
            var match = _inventoryNameRegex.Match(grn);
            if (!match.Success || !match.Groups["inventoryName"].Success)
            {
                return null;
            }
            return match.Groups["inventoryName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _itemNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetItemNameFromGrn(
            string grn
        )
        {
            var match = _itemNameRegex.Match(grn);
            if (!match.Success || !match.Groups["itemName"].Success)
            {
                return null;
            }
            return match.Groups["itemName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _itemSetNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetItemSetNameFromGrn(
            string grn
        )
        {
            var match = _itemSetNameRegex.Match(grn);
            if (!match.Success || !match.Groups["itemSetName"].Success)
            {
                return null;
            }
            return match.Groups["itemSetName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _referenceOfRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inventory:(?<namespaceName>.+):user:(?<userId>.+):inventory:(?<inventoryName>.+):item:(?<itemName>.+):itemSet:(?<itemSetName>.+):referenceOf:(?<referenceOf>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetReferenceOfFromGrn(
            string grn
        )
        {
            var match = _referenceOfRegex.Match(grn);
            if (!match.Success || !match.Groups["referenceOf"].Success)
            {
                return null;
            }
            return match.Groups["referenceOf"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReferenceOf FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReferenceOf()
                .WithReferenceOfId(!data.Keys.Contains("referenceOfId") || data["referenceOfId"] == null ? null : data["referenceOfId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["referenceOfId"] = ReferenceOfId,
                ["name"] = Name,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReferenceOfId != null) {
                writer.WritePropertyName("referenceOfId");
                writer.Write(ReferenceOfId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ReferenceOf;
            var diff = 0;
            if (ReferenceOfId == null && ReferenceOfId == other.ReferenceOfId)
            {
                // null and null
            }
            else
            {
                diff += ReferenceOfId.CompareTo(other.ReferenceOfId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ReferenceOfId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("referenceOf", "inventory.referenceOf.referenceOfId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("referenceOf", "inventory.referenceOf.name.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ReferenceOf {
                ReferenceOfId = ReferenceOfId,
                Name = Name,
            };
        }
    }
}