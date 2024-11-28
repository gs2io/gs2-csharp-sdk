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

namespace Gs2.Gs2Ranking2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class RankingReward : IComparable
	{
        public int? ThresholdRank { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; } = null!;
        public RankingReward WithThresholdRank(int? thresholdRank) {
            this.ThresholdRank = thresholdRank;
            return this;
        }
        public RankingReward WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public RankingReward WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RankingReward FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RankingReward()
                .WithThresholdRank(!data.Keys.Contains("thresholdRank") || data["thresholdRank"] == null ? null : (int?)(data["thresholdRank"].ToString().Contains(".") ? (int)double.Parse(data["thresholdRank"].ToString()) : int.Parse(data["thresholdRank"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["thresholdRank"] = ThresholdRank,
                ["metadata"] = Metadata,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ThresholdRank != null) {
                writer.WritePropertyName("thresholdRank");
                writer.Write((ThresholdRank.ToString().Contains(".") ? (int)double.Parse(ThresholdRank.ToString()) : int.Parse(ThresholdRank.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as RankingReward;
            var diff = 0;
            if (ThresholdRank == null && ThresholdRank == other.ThresholdRank)
            {
                // null and null
            }
            else
            {
                diff += (int)(ThresholdRank - other.ThresholdRank);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (AcquireActions == null && AcquireActions == other.AcquireActions)
            {
                // null and null
            }
            else
            {
                diff += AcquireActions.Length - other.AcquireActions.Length;
                for (var i = 0; i < AcquireActions.Length; i++)
                {
                    diff += AcquireActions[i].CompareTo(other.AcquireActions[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (ThresholdRank < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rankingReward", "ranking2.rankingReward.thresholdRank.error.invalid"),
                    });
                }
                if (ThresholdRank > 1001) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rankingReward", "ranking2.rankingReward.thresholdRank.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rankingReward", "ranking2.rankingReward.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("rankingReward", "ranking2.rankingReward.acquireActions.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new RankingReward {
                ThresholdRank = ThresholdRank,
                Metadata = Metadata,
                AcquireActions = AcquireActions.Clone() as Gs2.Core.Model.AcquireAction[],
            };
        }
    }
}