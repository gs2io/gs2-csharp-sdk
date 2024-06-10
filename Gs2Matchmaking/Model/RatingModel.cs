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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RatingModel : IComparable
	{
        public string RatingModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public int? InitialValue { set; get; } = null!;
        public int? Volatility { set; get; } = null!;
        public RatingModel WithRatingModelId(string ratingModelId) {
            this.RatingModelId = ratingModelId;
            return this;
        }
        public RatingModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public RatingModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RatingModel WithInitialValue(int? initialValue) {
            this.InitialValue = initialValue;
            return this;
        }
        public RatingModel WithVolatility(int? volatility) {
            this.Volatility = volatility;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<ratingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<ratingName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<ratingName>.+)",
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

        private static System.Text.RegularExpressions.Regex _ratingNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):matchmaking:(?<namespaceName>.+):model:(?<ratingName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRatingNameFromGrn(
            string grn
        )
        {
            var match = _ratingNameRegex.Match(grn);
            if (!match.Success || !match.Groups["ratingName"].Success)
            {
                return null;
            }
            return match.Groups["ratingName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RatingModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RatingModel()
                .WithRatingModelId(!data.Keys.Contains("ratingModelId") || data["ratingModelId"] == null ? null : data["ratingModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialValue(!data.Keys.Contains("initialValue") || data["initialValue"] == null ? null : (int?)(data["initialValue"].ToString().Contains(".") ? (int)double.Parse(data["initialValue"].ToString()) : int.Parse(data["initialValue"].ToString())))
                .WithVolatility(!data.Keys.Contains("volatility") || data["volatility"] == null ? null : (int?)(data["volatility"].ToString().Contains(".") ? (int)double.Parse(data["volatility"].ToString()) : int.Parse(data["volatility"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["ratingModelId"] = RatingModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["initialValue"] = InitialValue,
                ["volatility"] = Volatility,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RatingModelId != null) {
                writer.WritePropertyName("ratingModelId");
                writer.Write(RatingModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InitialValue != null) {
                writer.WritePropertyName("initialValue");
                writer.Write((InitialValue.ToString().Contains(".") ? (int)double.Parse(InitialValue.ToString()) : int.Parse(InitialValue.ToString())));
            }
            if (Volatility != null) {
                writer.WritePropertyName("volatility");
                writer.Write((Volatility.ToString().Contains(".") ? (int)double.Parse(Volatility.ToString()) : int.Parse(Volatility.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RatingModel;
            var diff = 0;
            if (RatingModelId == null && RatingModelId == other.RatingModelId)
            {
                // null and null
            }
            else
            {
                diff += RatingModelId.CompareTo(other.RatingModelId);
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
            if (InitialValue == null && InitialValue == other.InitialValue)
            {
                // null and null
            }
            else
            {
                diff += (int)(InitialValue - other.InitialValue);
            }
            if (Volatility == null && Volatility == other.Volatility)
            {
                // null and null
            }
            else
            {
                diff += (int)(Volatility - other.Volatility);
            }
            return diff;
        }

        public void Validate() {
            {
                if (RatingModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.ratingModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (InitialValue < 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.initialValue.error.invalid"),
                    });
                }
                if (InitialValue > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.initialValue.error.invalid"),
                    });
                }
            }
            {
                if (Volatility < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.volatility.error.invalid"),
                    });
                }
                if (Volatility > 20000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("ratingModel", "matchmaking.ratingModel.volatility.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new RatingModel {
                RatingModelId = RatingModelId,
                Name = Name,
                Metadata = Metadata,
                InitialValue = InitialValue,
                Volatility = Volatility,
            };
        }
    }
}