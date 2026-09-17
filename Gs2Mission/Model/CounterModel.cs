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

namespace Gs2.Gs2Mission.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class CounterModel : IComparable
	{
        public string CounterId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Mission.Model.CounterScopeModel[] Scopes { set; get; }
        public string ChallengePeriodEventId { set; get; }
        public CounterModel WithCounterId(string counterId) {
            this.CounterId = counterId;
            return this;
        }
        public CounterModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public CounterModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CounterModel WithScopes(Gs2.Gs2Mission.Model.CounterScopeModel[] scopes) {
            this.Scopes = scopes;
            return this;
        }
        public CounterModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
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
        public static CounterModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new CounterModel()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScopes(!data.Keys.Contains("scopes") || data["scopes"] == null || !data["scopes"].IsArray ? null : data["scopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.CounterScopeModel.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
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
                ["scopes"] = scopesJsonData,
                ["challengePeriodEventId"] = ChallengePeriodEventId,
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
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CounterModel;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type CounterModel.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(CounterId, other.CounterId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(Scopes, other.Scopes);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(ChallengePeriodEventId, other.ChallengePeriodEventId);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (CounterId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.counterId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (Scopes.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.scopes.error.tooFew"),
                    });
                }
                if (Scopes.Length > 20) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.scopes.error.tooMany"),
                    });
                }
            }
            {
                if (ChallengePeriodEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("counterModel", "mission.counterModel.challengePeriodEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new CounterModel {
                CounterId = CounterId,
                Name = Name,
                Metadata = Metadata,
                Scopes = Scopes?.Clone() as Gs2.Gs2Mission.Model.CounterScopeModel[],
                ChallengePeriodEventId = ChallengePeriodEventId,
            };
        }
    }
}