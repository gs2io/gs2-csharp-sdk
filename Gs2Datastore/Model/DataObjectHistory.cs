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

namespace Gs2.Gs2Datastore.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class DataObjectHistory : IComparable
	{
        public string DataObjectHistoryId { set; get; }
        public string DataObjectName { set; get; }
        public string Generation { set; get; }
        public long? ContentLength { set; get; }
        public long? CreatedAt { set; get; }

        public DataObjectHistory WithDataObjectHistoryId(string dataObjectHistoryId) {
            this.DataObjectHistoryId = dataObjectHistoryId;
            return this;
        }

        public DataObjectHistory WithDataObjectName(string dataObjectName) {
            this.DataObjectName = dataObjectName;
            return this;
        }

        public DataObjectHistory WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

        public DataObjectHistory WithContentLength(long? contentLength) {
            this.ContentLength = contentLength;
            return this;
        }

        public DataObjectHistory WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
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

        private static System.Text.RegularExpressions.Regex _dataObjectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetDataObjectNameFromGrn(
            string grn
        )
        {
            var match = _dataObjectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["dataObjectName"].Success)
            {
                return null;
            }
            return match.Groups["dataObjectName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _generationRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):datastore:(?<namespaceName>.+):user:(?<userId>.+):data:(?<dataObjectName>.+):history:(?<generation>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetGenerationFromGrn(
            string grn
        )
        {
            var match = _generationRegex.Match(grn);
            if (!match.Success || !match.Groups["generation"].Success)
            {
                return null;
            }
            return match.Groups["generation"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DataObjectHistory FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DataObjectHistory()
                .WithDataObjectHistoryId(!data.Keys.Contains("dataObjectHistoryId") || data["dataObjectHistoryId"] == null ? null : data["dataObjectHistoryId"].ToString())
                .WithDataObjectName(!data.Keys.Contains("dataObjectName") || data["dataObjectName"] == null ? null : data["dataObjectName"].ToString())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString())
                .WithContentLength(!data.Keys.Contains("contentLength") || data["contentLength"] == null ? null : (long?)long.Parse(data["contentLength"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)long.Parse(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["dataObjectHistoryId"] = DataObjectHistoryId,
                ["dataObjectName"] = DataObjectName,
                ["generation"] = Generation,
                ["contentLength"] = ContentLength,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DataObjectHistoryId != null) {
                writer.WritePropertyName("dataObjectHistoryId");
                writer.Write(DataObjectHistoryId.ToString());
            }
            if (DataObjectName != null) {
                writer.WritePropertyName("dataObjectName");
                writer.Write(DataObjectName.ToString());
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            if (ContentLength != null) {
                writer.WritePropertyName("contentLength");
                writer.Write(long.Parse(ContentLength.ToString()));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write(long.Parse(CreatedAt.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as DataObjectHistory;
            var diff = 0;
            if (DataObjectHistoryId == null && DataObjectHistoryId == other.DataObjectHistoryId)
            {
                // null and null
            }
            else
            {
                diff += DataObjectHistoryId.CompareTo(other.DataObjectHistoryId);
            }
            if (DataObjectName == null && DataObjectName == other.DataObjectName)
            {
                // null and null
            }
            else
            {
                diff += DataObjectName.CompareTo(other.DataObjectName);
            }
            if (Generation == null && Generation == other.Generation)
            {
                // null and null
            }
            else
            {
                diff += Generation.CompareTo(other.Generation);
            }
            if (ContentLength == null && ContentLength == other.ContentLength)
            {
                // null and null
            }
            else
            {
                diff += (int)(ContentLength - other.ContentLength);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }
    }
}