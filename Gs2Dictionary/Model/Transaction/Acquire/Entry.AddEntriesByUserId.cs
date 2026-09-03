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
using System.Linq;
using System.Numerics;
using Gs2.Core.Util; /* diff +++ */
using Gs2.Gs2Dictionary.Request;

namespace Gs2.Gs2Dictionary.Model.Transaction
{
    public static partial class EntryExt
    {
        public static bool IsExecutable(
/* diff --- start
            this Entry self,
 diff --- end */
            this Entry[] self, /* diff +++ */
            AddEntriesByUserIdRequest request
        ) {
            try {
                self.SpeculativeExecution(request);
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

/* diff --- start
        public static Entry SpeculativeExecution(
            this Entry self,
 diff --- end */
/* diff +++ start */
        public static Entry[] SpeculativeExecution(
            this Entry[] self,
/* diff +++ end */
            AddEntriesByUserIdRequest request
        ) {
/* diff --- start
//#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Dictionary:AddEntriesByUserId");
//#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Dictionary:AddEntriesByUserId");
//#endif
            return self.Clone() as Entry;
 diff --- end */
/* diff +++ start */
            return self.SpeculativeExecutionAt(
                request,
                "{region}",
                "{ownerId}",
                UnixTime.ToUnixTime(DateTime.Now)
            );
        }

        public static Entry[] SpeculativeExecutionAt(
            this Entry[] self,
            AddEntriesByUserIdRequest request,
            string region,
            string ownerId,
            long currentTimeMillis
        ) {
            if (self == null ||
                request == null ||
                self.Any(v => v?.Name == null)) {
                throw new NullReferenceException();
            }
            if (string.IsNullOrEmpty(region) || string.IsNullOrEmpty(ownerId)) {
                throw new NullReferenceException();
            }
            var items = self.ToList();
            var names = self.Select(v => v.Name).ToHashSet();
            foreach (var entryModelName in request.EntryModelNames ?? Array.Empty<string>()) {
                if (names.Add(entryModelName)) {
                    items.Add(new Entry {
                        EntryId = $"grn:gs2:{region}:{ownerId}:dictionary:{request.NamespaceName}:user:{request.UserId}:entry:{entryModelName}",
                        UserId = request.UserId,
                        Name = entryModelName,
                        AcquiredAt = currentTimeMillis,
                    });
                }
            }
            return items.ToArray();
/* diff +++ end */
        }

        public static AddEntriesByUserIdRequest Rate(
            this AddEntriesByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class AddEntriesByUserIdRequestExt
    {
        public static AddEntriesByUserIdRequest Rate(
            this AddEntriesByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}
