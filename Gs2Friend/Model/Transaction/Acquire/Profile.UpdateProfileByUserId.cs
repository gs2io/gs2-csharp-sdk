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
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
/* diff --- start
using System.Linq;
 diff --- end */
using System.Numerics;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Friend.Request;

namespace Gs2.Gs2Friend.Model.Transaction
{
    public static partial class ProfileExt
    {
        public static bool IsExecutable(
            this Profile self,
            UpdateProfileByUserIdRequest request
        ) {
/* diff --- start
            var changed = self.SpeculativeExecution(request);
 diff --- end */
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
                self.SpeculativeExecution(request); /* diff +++ */
                return true;
            }
/* diff --- start
            catch (Gs2Exception) {
 diff --- end */
            catch (System.Exception) { /* diff +++ */
                return false;
            }
        }

        public static Profile SpeculativeExecution(
            this Profile self,
            UpdateProfileByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Friend:UpdateProfileByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Friend:UpdateProfileByUserId");
//#endif
            return self.Clone() as Profile;
 diff --- end */
/* diff +++ start */
            var now = UnixTime.ToUnixTime(DateTime.Now);
            return self.SpeculativeExecutionAt(request, now, now);
        }

        public static Profile SpeculativeExecutionAt(
            this Profile self,
            UpdateProfileByUserIdRequest request,
            long currentTimeMillis,
            long createdAtMillis,
            string region = null,
            string ownerId = null
        ) {
            if (request == null) {
                throw new InvalidOperationException("request is unavailable");
            }
            Profile clone;
            if (self == null) {
                if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(ownerId)) {
                    throw new InvalidOperationException("profile is unavailable");
                }
                clone = new Profile {
                    ProfileId = $"grn:gs2:{region}:{ownerId}:friend:{request.NamespaceName}:user:{request.UserId}",
                    UserId = request.UserId,
                    CreatedAt = createdAtMillis,
                    UpdatedAt = createdAtMillis,
                    Revision = 0,
                };
            }
            else {
                clone = self.Clone() as Profile;
                if (clone == null) {
                    throw new InvalidOperationException("profile is unavailable");
                }
            }
            clone.PublicProfile = request.PublicProfile;
            clone.FollowerProfile = request.FollowerProfile;
            clone.FriendProfile = request.FriendProfile;
            clone.UpdatedAt = currentTimeMillis;
            clone.Revision = 0;
            return clone;
/* diff +++ end */
        }

        public static UpdateProfileByUserIdRequest Rate(
            this UpdateProfileByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:UpdateProfileByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class UpdateProfileByUserIdRequestExt
    {
        public static UpdateProfileByUserIdRequest Rate(
            this UpdateProfileByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Friend:UpdateProfileByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}