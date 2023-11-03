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
