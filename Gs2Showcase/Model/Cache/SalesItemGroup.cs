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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using Gs2.Core.Domain;
using Gs2.Core.Util;

namespace Gs2.Gs2Showcase.Model.Cache
{
    public static partial class SalesItemGroupExt
    {
        public static string CacheParentKey(
            this SalesItemGroup self,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId
        ) {
            return string.Join(
                ":",
                "showcase",
                namespaceName ?? "null",
                userId ?? "null",
                showcaseName ?? "null",
                displayItemId ?? "null",
                "SalesItemGroup"
            );
        }

        public static string CacheKey(
            this SalesItemGroup self
        ) {
            return "Singleton";
        }

        public static void PutCache(
            this SalesItemGroup self,
            CacheDatabase cache,
            string namespaceName,
            string userId,
            string showcaseName,
            string displayItemId
        ) {
            cache.Put(
                self.CacheParentKey(
                    namespaceName,
                    userId,
                    showcaseName,
                    displayItemId
                ),
                self.CacheKey(
                ),
                self,
                UnixTime.ToUnixTime(DateTime.Now) + 1000 * 60 * Gs2.Core.Domain.Gs2.DefaultCacheMinutes
            );
        }
    }
}