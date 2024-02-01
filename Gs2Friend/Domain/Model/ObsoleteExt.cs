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
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Friend.Model;
using Gs2.Gs2Friend.Model.Cache;
using Gs2.Gs2Friend.Domain.Iterator;
using Gs2.Gs2Friend.Domain.Model;
using Gs2.Gs2Friend.Request;
using Gs2.Gs2Friend.Result;
using Gs2.Gs2Auth.Model;
using Gs2.Util.LitJson;
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Util;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using System.Collections.Generic;
#endif
#else
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif

#pragma warning disable 1998
#pragma warning disable CS0169, CS0168

namespace Gs2.Core.Domain
{
    public static partial class ObsoleteExt
    {
        [Obsolete("FollowUser() -> Follow().FollowUser()")]
        public static FollowUserAccessTokenDomain FollowUser(
            this UserAccessTokenDomain self,
            string targetUserId,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).FollowUser(
                targetUserId
            );
        }

        [Obsolete("FollowUser() -> Follow().FollowUser()")]
        public static FollowUserDomain FollowUser(
            this UserDomain self,
            string targetUserId,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).FollowUser(
                targetUserId
            );
        }

#if UNITY_2017_1_OR_NEWER
        [Obsolete("Follows() -> Follow().Follows()")]
        public static Gs2Iterator<FollowUser> Follows(
            this UserAccessTokenDomain self,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).Follows(
            );
        }

        [Obsolete("Follows() -> Follow().Follows()")]
        public static Gs2Iterator<FollowUser> Follows(
            this UserDomain self,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).Follows(
            );
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
        [Obsolete("FollowsAsync() -> Follow().FollowsAsync()")]
    #if UNITY_2017_1_OR_NEWER
        public static IUniTaskAsyncEnumerable<FollowUser> FollowsAsync(
    #else
        public static DescribeFollowsIterator FollowsAsync(
    #endif
            this UserAccessTokenDomain self,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).FollowsAsync(
            );
        }

        [Obsolete("FollowsAsync() -> Follow().FollowsAsync()")]
    #if UNITY_2017_1_OR_NEWER
        public static IUniTaskAsyncEnumerable<FollowUser> FollowsAsync(
    #else
        public static DescribeFollowsByUserIdIterator FollowsAsync(
    #endif
            this UserDomain self,
            bool? withProfile
        ) {
            return self.Follow(
                withProfile
            ).FollowsAsync(
            );
        }
#endif
        
        [Obsolete("SubscribeFollows() -> Follow().SubscribeFollows()")]
        public static ulong SubscribeFollows(
            this UserAccessTokenDomain self,
            Action<FollowUser[]> callback,
            bool? withProfile = null
        ) {
            return self.Follow(
                withProfile
            ).SubscribeFollows(
                callback
            );
        }

        [Obsolete("SubscribeFollows() -> Follow().SubscribeFollows()")]
        public static ulong SubscribeFollows(
            this UserDomain self,
            Action<FollowUser[]> callback,
            bool? withProfile = null
        ) {
            return self.Follow(
                withProfile
            ).SubscribeFollows(
                callback
            );
        }
        
        [Obsolete("UnsubscribeFollows() -> Follow().UnsubscribeFollows()")]
        public static void UnsubscribeFollows(
            this UserAccessTokenDomain self,
            ulong callbackId,
            bool? withProfile = null
        ) {
            self.Follow(
                withProfile
            ).UnsubscribeFollows(
                callbackId
            );
        }

        [Obsolete("UnsubscribeFollows() -> Follow().UnsubscribeFollows()")]
        public static void UnsubscribeFollows(
            this UserDomain self,
            ulong callbackId,
            bool? withProfile = null
        ) {
            self.Follow(
                withProfile
            ).UnsubscribeFollows(
                callbackId
            );
        }
    }
}
