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

namespace Gs2.Gs2Inbox.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Received : IComparable
	{
        public string ReceivedId { set; get; }
        public string UserId { set; get; }
        public string[] ReceivedGlobalMessageNames { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }

        public Received WithReceivedId(string receivedId) {
            this.ReceivedId = receivedId;
            return this;
        }

        public Received WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public Received WithReceivedGlobalMessageNames(string[] receivedGlobalMessageNames) {
            this.ReceivedGlobalMessageNames = receivedGlobalMessageNames;
            return this;
        }

        public Received WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public Received WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):inbox:(?<namespaceName>.+):user:(?<userId>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Received FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Received()
                .WithReceivedId(!data.Keys.Contains("receivedId") || data["receivedId"] == null ? null : data["receivedId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithReceivedGlobalMessageNames(!data.Keys.Contains("receivedGlobalMessageNames") || data["receivedGlobalMessageNames"] == null ? new string[]{} : data["receivedGlobalMessageNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)long.Parse(data["updatedAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["receivedId"] = ReceivedId,
                ["userId"] = UserId,
                ["receivedGlobalMessageNames"] = new JsonData(ReceivedGlobalMessageNames == null ? new JsonData[]{} :
                        ReceivedGlobalMessageNames.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ReceivedId != null) {
                writer.WritePropertyName("receivedId");
                writer.Write(ReceivedId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ReceivedGlobalMessageNames != null) {
                writer.WritePropertyName("receivedGlobalMessageNames");
                writer.WriteArrayStart();
                foreach (var receivedGlobalMessageName in ReceivedGlobalMessageNames)
                {
                    if (receivedGlobalMessageName != null) {
                        writer.Write(receivedGlobalMessageName.ToString());
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
            var other = obj as Received;
            var diff = 0;
            if (ReceivedId == null && ReceivedId == other.ReceivedId)
            {
                // null and null
            }
            else
            {
                diff += ReceivedId.CompareTo(other.ReceivedId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (ReceivedGlobalMessageNames == null && ReceivedGlobalMessageNames == other.ReceivedGlobalMessageNames)
            {
                // null and null
            }
            else
            {
                diff += ReceivedGlobalMessageNames.Length - other.ReceivedGlobalMessageNames.Length;
                for (var i = 0; i < ReceivedGlobalMessageNames.Length; i++)
                {
                    diff += ReceivedGlobalMessageNames[i].CompareTo(other.ReceivedGlobalMessageNames[i]);
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