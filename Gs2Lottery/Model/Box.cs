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

namespace Gs2.Gs2Lottery.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Box : IComparable
	{
        public string BoxId { set; get; }
        public string PrizeTableName { set; get; }
        public string UserId { set; get; }
        public int[] DrawnIndexes { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Box WithBoxId(string boxId) {
            this.BoxId = boxId;
            return this;
        }

        public Box WithPrizeTableName(string prizeTableName) {
            this.PrizeTableName = prizeTableName;
            return this;
        }

        public Box WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Box WithDrawnIndexes(int[] drawnIndexes) {
            this.DrawnIndexes = drawnIndexes;
            return this;
        }

        public Box WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Box WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):user:(?<userId>.+):box:table:(?<prizeTableName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):user:(?<userId>.+):box:table:(?<prizeTableName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):user:(?<userId>.+):box:table:(?<prizeTableName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):user:(?<userId>.+):box:table:(?<prizeTableName>.+)",
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

        private static System.Text.RegularExpressions.Regex _prizeTableNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):lottery:(?<namespaceName>.+):user:(?<userId>.+):box:table:(?<prizeTableName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetPrizeTableNameFromGrn(
            string grn
        )
        {
            var match = _prizeTableNameRegex.Match(grn);
            if (!match.Success || !match.Groups["prizeTableName"].Success)
            {
                return null;
            }
            return match.Groups["prizeTableName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Box FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Box()
                .WithBoxId(!data.Keys.Contains("boxId") || data["boxId"] == null ? null : data["boxId"].ToString())
                .WithPrizeTableName(!data.Keys.Contains("prizeTableName") || data["prizeTableName"] == null ? null : data["prizeTableName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithDrawnIndexes(!data.Keys.Contains("drawnIndexes") || data["drawnIndexes"] == null ? new int[]{} : data["drawnIndexes"].Cast<JsonData>().Select(v => {
                    return int.Parse(v.ToString());
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["boxId"] = BoxId,
                ["prizeTableName"] = PrizeTableName,
                ["userId"] = UserId,
                ["drawnIndexes"] = new JsonData(DrawnIndexes == null ? new JsonData[]{} :
                        DrawnIndexes.Select(v => {
                            return new JsonData((int?)int.Parse(v.ToString()));
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (BoxId != null) {
                writer.WritePropertyName("boxId");
                writer.Write(BoxId.ToString());
            }
            if (PrizeTableName != null) {
                writer.WritePropertyName("prizeTableName");
                writer.Write(PrizeTableName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (DrawnIndexes != null) {
                writer.WritePropertyName("drawnIndexes");
                writer.WriteArrayStart();
                foreach (var drawnIndex in DrawnIndexes)
                {
                    if (drawnIndex != null) {
                        writer.Write(int.Parse(drawnIndex.ToString()));
                    }
                }
                writer.WriteArrayEnd();
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write(long.Parse(UpdatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Box;
            var diff = 0;
            if (BoxId == null && BoxId == other.BoxId)
            {
                // null and null
            }
            else
            {
                diff += BoxId.CompareTo(other.BoxId);
            }
            if (PrizeTableName == null && PrizeTableName == other.PrizeTableName)
            {
                // null and null
            }
            else
            {
                diff += PrizeTableName.CompareTo(other.PrizeTableName);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (DrawnIndexes == null && DrawnIndexes == other.DrawnIndexes)
            {
                // null and null
            }
            else
            {
                diff += DrawnIndexes.Length - other.DrawnIndexes.Length;
                for (var i = 0; i < DrawnIndexes.Length; i++)
                {
                    diff += (int)(DrawnIndexes[i] - other.DrawnIndexes[i]);
                }
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            return diff;
        }
    }
}