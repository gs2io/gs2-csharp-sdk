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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CounterModelMaster : IComparable
	{
        public string CounterId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public string Description { set; get; } = null!;
        public Gs2.Gs2Mission.Model.CounterScopeModel[] Scopes { set; get; } = null!;
        public string ChallengePeriodEventId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public CounterModelMaster WithCounterId(string counterId) {
            this.CounterId = counterId;
            return this;
        }
        public CounterModelMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public CounterModelMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CounterModelMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CounterModelMaster WithScopes(Gs2.Gs2Mission.Model.CounterScopeModel[] scopes) {
            this.Scopes = scopes;
            return this;
        }
        public CounterModelMaster WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }
        public CounterModelMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public CounterModelMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public CounterModelMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):counter:(?<counterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):counter:(?<counterName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):counter:(?<counterName>.+)",
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

        private static System.Text.RegularExpressions.Regex _counterNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):mission:(?<namespaceName>.+):counter:(?<counterName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetCounterNameFromGrn(
            string grn
        )
        {
            var match = _counterNameRegex.Match(grn);
            if (!match.Success || !match.Groups["counterName"].Success)
            {
                return null;
            }
            return match.Groups["counterName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CounterModelMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CounterModelMaster()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithScopes(!data.Keys.Contains("scopes") || data["scopes"] == null || !data["scopes"].IsArray ? new Gs2.Gs2Mission.Model.CounterScopeModel[]{} : data["scopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.CounterScopeModel.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData scopesJsonData = null;
            if (Scopes != null && Scopes.Length > 0)
            {
                scopesJsonData = new JsonData();
                foreach (var scope in Scopes)
                {
                    scopesJsonData.Add(scope.ToJson());
                }
            }
            return new JsonData {
                ["counterId"] = CounterId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["description"] = Description,
                ["scopes"] = scopesJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CounterId != null) {
                writer.WritePropertyName("counterId");
                writer.Write(CounterId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Scopes != null) {
                writer.WritePropertyName("scopes");
                writer.WriteArrayStart();
                foreach (var scope in Scopes)
                {
                    if (scope != null) {
                        scope.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CounterModelMaster;
            var diff = 0;
            if (CounterId == null && CounterId == other.CounterId)
            {
                // null and null
            }
            else
            {
                diff += CounterId.CompareTo(other.CounterId);
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
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Scopes == null && Scopes == other.Scopes)
            {
                // null and null
            }
            else
            {
                diff += Scopes.Length - other.Scopes.Length;
                for (var i = 0; i < Scopes.Length; i++)
                {
                    diff += Scopes[i].CompareTo(other.Scopes[i]);
                }
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
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
                if (CounterId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.counterId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Scopes.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.scopes.error.tooFew"),
                    });
                }
                if (Scopes.Length > 20) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.scopes.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModelMaster", "mission.counterModelMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new CounterModelMaster {
                CounterId = CounterId,
                Name = Name,
                Metadata = Metadata,
                Description = Description,
                Scopes = Scopes.Clone() as Gs2.Gs2Mission.Model.CounterScopeModel[],
                ChallengePeriodEventId = ChallengePeriodEventId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}