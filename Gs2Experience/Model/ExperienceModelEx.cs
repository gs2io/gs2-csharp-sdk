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
using System.Linq;

namespace Gs2.Gs2Experience.Model
{
    public static class ExperienceModelEx
    {
        public static Status RecalculateStatus(
            this ExperienceModel self,
            Status source,
            long experienceValue,
            long rankCapValue
        ) {
            if (self?.MaxRankCap == null || source?.Clone() is not Status clone) {
                throw new NullReferenceException();
            }

            var values = self.RankThreshold?.Values ?? Array.Empty<long>();
            if (rankCapValue <= 1 || values.Length == 0) {
                experienceValue = 0;
            }
            else {
                var rankCapExperienceValue = values[values.Length - 1];
                if (rankCapValue - 1 < values.Length) {
                    rankCapExperienceValue = values[(int)rankCapValue - 2];
                }
                if (experienceValue > rankCapExperienceValue) {
                    experienceValue = rankCapExperienceValue;
                }
            }

            rankCapValue = Math.Min(rankCapValue, self.MaxRankCap.Value);
            long rankValue = 1;
            long nextRankUpExperienceValue = 0;
            foreach (var thresholdValue in values) {
                if (experienceValue < thresholdValue) {
                    if (rankCapValue > rankValue) {
                        nextRankUpExperienceValue = thresholdValue;
                    }
                    break;
                }
                rankValue++;
            }

            clone.ExperienceValue = experienceValue;
            clone.RankValue = rankValue;
            clone.RankCapValue = rankCapValue;
            clone.NextRankUpExperienceValue = nextRankUpExperienceValue;
            clone.Revision = 0;
            return clone;
        }

        public static long Rank(this ExperienceModel self, Status status) {
            return Math.Min(self.RankThreshold.Values.Count(v => v <= (status.ExperienceValue ?? 0)) + 1, status.RankCapValue ?? 0);
        }

        public static long NextRankExperienceValue(this ExperienceModel self, Status status) {
            var newRank = self.Rank(status);
            if (newRank == status.RankCapValue) {
                return 0;
            }
            return self.RankThreshold.Values[(int)Math.Min(newRank, status.RankCapValue ?? 0)-1];
        }
    }
}
