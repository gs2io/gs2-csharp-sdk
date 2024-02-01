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
 *
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Core.Util;
using Gs2.Gs2Dictionary.Request;

namespace Gs2.Gs2Dictionary.Model.Transaction
{
    public static partial class EntryExt
    {
        public static bool IsExecutable(
            this Entry[] self,
            AddEntriesByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                foreach (var v in changed) {
                    v.Validate();
                }
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static Entry[] SpeculativeExecution(
            this Entry[] self,
            AddEntriesByUserIdRequest request
        ) {
            var items = self.ToList();
            foreach (var entryModelName in request.EntryModelNames) {
                if (!self.Select(v => v.Name).ToList().Contains(entryModelName)) {
                    items.Add(new Entry {
                        UserId = request.UserId,
                        Name = entryModelName,
                        AcquiredAt = UnixTime.ToUnixTime(DateTime.Now),
                    });
                }
            }
            return items.ToArray();
        }

        public static AddEntriesByUserIdRequest Rate(
            this AddEntriesByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:AddEntriesByUserId");
        }
    }

    public static partial class AddEntriesByUserIdRequestExt
    {
        public static AddEntriesByUserIdRequest Rate(
            this AddEntriesByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:AddEntriesByUserId");
        }
    }
}