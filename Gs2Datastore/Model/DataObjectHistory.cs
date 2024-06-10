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
        public string DataObjectHistoryId { set; get; } = null!;
        public string DataObjectName { set; get; } = null!;
        public string Generation { set; get; } = null!;
        public long? ContentLength { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public DataObjectHistory WithRevision(long? revision) {
            this.Revision = revision;
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
                .WithContentLength(!data.Keys.Contains("contentLength") || data["contentLength"] == null ? null : (long?)(data["contentLength"].ToString().Contains(".") ? (long)double.Parse(data["contentLength"].ToString()) : long.Parse(data["contentLength"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["dataObjectHistoryId"] = DataObjectHistoryId,
                ["dataObjectName"] = DataObjectName,
                ["generation"] = Generation,
                ["contentLength"] = ContentLength,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
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
                writer.Write((ContentLength.ToString().Contains(".") ? (long)double.Parse(ContentLength.ToString()) : long.Parse(ContentLength.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
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
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (DataObjectHistoryId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.dataObjectHistoryId.error.tooLong"),
                    });
                }
            }
            {
                if (DataObjectName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.dataObjectName.error.tooLong"),
                    });
                }
            }
            {
                if (Generation.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.generation.error.tooLong"),
                    });
                }
            }
            {
                if (ContentLength < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.contentLength.error.invalid"),
                    });
                }
                if (ContentLength > 10485760) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.contentLength.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("dataObjectHistory", "datastore.dataObjectHistory.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new DataObjectHistory {
                DataObjectHistoryId = DataObjectHistoryId,
                DataObjectName = DataObjectName,
                Generation = Generation,
                ContentLength = ContentLength,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}