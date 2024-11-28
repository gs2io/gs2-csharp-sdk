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

namespace Gs2.Gs2Ranking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class GlobalRankingSetting : IComparable
	{
        public bool? UniqueByUserId { set; get; } = null!;
        public int? CalculateIntervalMinutes { set; get; } = null!;
        public Gs2.Gs2Ranking.Model.FixedTiming CalculateFixedTiming { set; get; } = null!;
        public Gs2.Gs2Ranking.Model.Scope[] AdditionalScopes { set; get; } = null!;
        public string[] IgnoreUserIds { set; get; } = null!;
        public string Generation { set; get; } = null!;
        public GlobalRankingSetting WithUniqueByUserId(bool? uniqueByUserId) {
            this.UniqueByUserId = uniqueByUserId;
            return this;
        }
        public GlobalRankingSetting WithCalculateIntervalMinutes(int? calculateIntervalMinutes) {
            this.CalculateIntervalMinutes = calculateIntervalMinutes;
            return this;
        }
        public GlobalRankingSetting WithCalculateFixedTiming(Gs2.Gs2Ranking.Model.FixedTiming calculateFixedTiming) {
            this.CalculateFixedTiming = calculateFixedTiming;
            return this;
        }
        public GlobalRankingSetting WithAdditionalScopes(Gs2.Gs2Ranking.Model.Scope[] additionalScopes) {
            this.AdditionalScopes = additionalScopes;
            return this;
        }
        public GlobalRankingSetting WithIgnoreUserIds(string[] ignoreUserIds) {
            this.IgnoreUserIds = ignoreUserIds;
            return this;
        }
        public GlobalRankingSetting WithGeneration(string generation) {
            this.Generation = generation;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GlobalRankingSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GlobalRankingSetting()
                .WithUniqueByUserId(!data.Keys.Contains("uniqueByUserId") || data["uniqueByUserId"] == null ? null : (bool?)bool.Parse(data["uniqueByUserId"].ToString()))
                .WithCalculateIntervalMinutes(!data.Keys.Contains("calculateIntervalMinutes") || data["calculateIntervalMinutes"] == null ? null : (int?)(data["calculateIntervalMinutes"].ToString().Contains(".") ? (int)double.Parse(data["calculateIntervalMinutes"].ToString()) : int.Parse(data["calculateIntervalMinutes"].ToString())))
                .WithCalculateFixedTiming(!data.Keys.Contains("calculateFixedTiming") || data["calculateFixedTiming"] == null ? null : Gs2.Gs2Ranking.Model.FixedTiming.FromJson(data["calculateFixedTiming"]))
                .WithAdditionalScopes(!data.Keys.Contains("additionalScopes") || data["additionalScopes"] == null || !data["additionalScopes"].IsArray ? null : data["additionalScopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Ranking.Model.Scope.FromJson(v);
                }).ToArray())
                .WithIgnoreUserIds(!data.Keys.Contains("ignoreUserIds") || data["ignoreUserIds"] == null || !data["ignoreUserIds"].IsArray ? null : data["ignoreUserIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithGeneration(!data.Keys.Contains("generation") || data["generation"] == null ? null : data["generation"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData additionalScopesJsonData = null;
            if (AdditionalScopes != null && AdditionalScopes.Length > 0)
            {
                additionalScopesJsonData = new JsonData();
                foreach (var additionalScope in AdditionalScopes)
                {
                    additionalScopesJsonData.Add(additionalScope.ToJson());
                }
            }
            JsonData ignoreUserIdsJsonData = null;
            if (IgnoreUserIds != null && IgnoreUserIds.Length > 0)
            {
                ignoreUserIdsJsonData = new JsonData();
                foreach (var ignoreUserId in IgnoreUserIds)
                {
                    ignoreUserIdsJsonData.Add(ignoreUserId);
                }
            }
            return new JsonData {
                ["uniqueByUserId"] = UniqueByUserId,
                ["calculateIntervalMinutes"] = CalculateIntervalMinutes,
                ["calculateFixedTiming"] = CalculateFixedTiming?.ToJson(),
                ["additionalScopes"] = additionalScopesJsonData,
                ["ignoreUserIds"] = ignoreUserIdsJsonData,
                ["generation"] = Generation,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (UniqueByUserId != null) {
                writer.WritePropertyName("uniqueByUserId");
                writer.Write(bool.Parse(UniqueByUserId.ToString()));
            }
            if (CalculateIntervalMinutes != null) {
                writer.WritePropertyName("calculateIntervalMinutes");
                writer.Write((CalculateIntervalMinutes.ToString().Contains(".") ? (int)double.Parse(CalculateIntervalMinutes.ToString()) : int.Parse(CalculateIntervalMinutes.ToString())));
            }
            if (CalculateFixedTiming != null) {
                writer.WritePropertyName("calculateFixedTiming");
                CalculateFixedTiming.WriteJson(writer);
            }
            if (AdditionalScopes != null) {
                writer.WritePropertyName("additionalScopes");
                writer.WriteArrayStart();
                foreach (var additionalScope in AdditionalScopes)
                {
                    if (additionalScope != null) {
                        additionalScope.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IgnoreUserIds != null) {
                writer.WritePropertyName("ignoreUserIds");
                writer.WriteArrayStart();
                foreach (var ignoreUserId in IgnoreUserIds)
                {
                    if (ignoreUserId != null) {
                        writer.Write(ignoreUserId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Generation != null) {
                writer.WritePropertyName("generation");
                writer.Write(Generation.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as GlobalRankingSetting;
            var diff = 0;
            if (UniqueByUserId == null && UniqueByUserId == other.UniqueByUserId)
            {
                // null and null
            }
            else
            {
                diff += UniqueByUserId == other.UniqueByUserId ? 0 : 1;
            }
            if (CalculateIntervalMinutes == null && CalculateIntervalMinutes == other.CalculateIntervalMinutes)
            {
                // null and null
            }
            else
            {
                diff += (int)(CalculateIntervalMinutes - other.CalculateIntervalMinutes);
            }
            if (CalculateFixedTiming == null && CalculateFixedTiming == other.CalculateFixedTiming)
            {
                // null and null
            }
            else
            {
                diff += CalculateFixedTiming.CompareTo(other.CalculateFixedTiming);
            }
            if (AdditionalScopes == null && AdditionalScopes == other.AdditionalScopes)
            {
                // null and null
            }
            else
            {
                diff += AdditionalScopes.Length - other.AdditionalScopes.Length;
                for (var i = 0; i < AdditionalScopes.Length; i++)
                {
                    diff += AdditionalScopes[i].CompareTo(other.AdditionalScopes[i]);
                }
            }
            if (IgnoreUserIds == null && IgnoreUserIds == other.IgnoreUserIds)
            {
                // null and null
            }
            else
            {
                diff += IgnoreUserIds.Length - other.IgnoreUserIds.Length;
                for (var i = 0; i < IgnoreUserIds.Length; i++)
                {
                    diff += IgnoreUserIds[i].CompareTo(other.IgnoreUserIds[i]);
                }
            }
            if (Generation == null && Generation == other.Generation)
            {
                // null and null
            }
            else
            {
                diff += Generation.CompareTo(other.Generation);
            }
            return diff;
        }

        public void Validate() {
            {
            }
            {
                if (CalculateIntervalMinutes < 15) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingSetting", "ranking.globalRankingSetting.calculateIntervalMinutes.error.invalid"),
                    });
                }
                if (CalculateIntervalMinutes > 1440) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingSetting", "ranking.globalRankingSetting.calculateIntervalMinutes.error.invalid"),
                    });
                }
            }
            {
            }
            {
                if (AdditionalScopes.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingSetting", "ranking.globalRankingSetting.additionalScopes.error.tooMany"),
                    });
                }
            }
            {
                if (IgnoreUserIds.Length > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingSetting", "ranking.globalRankingSetting.ignoreUserIds.error.tooMany"),
                    });
                }
            }
            {
                if (Generation.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("globalRankingSetting", "ranking.globalRankingSetting.generation.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new GlobalRankingSetting {
                UniqueByUserId = UniqueByUserId,
                CalculateIntervalMinutes = CalculateIntervalMinutes,
                CalculateFixedTiming = CalculateFixedTiming.Clone() as Gs2.Gs2Ranking.Model.FixedTiming,
                AdditionalScopes = AdditionalScopes.Clone() as Gs2.Gs2Ranking.Model.Scope[],
                IgnoreUserIds = IgnoreUserIds.Clone() as string[],
                Generation = Generation,
            };
        }
    }
}