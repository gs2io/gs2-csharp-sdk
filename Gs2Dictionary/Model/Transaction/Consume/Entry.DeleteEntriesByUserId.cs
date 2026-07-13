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
using Gs2.Core.Exception;
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
            DeleteEntriesByUserIdRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
/* diff --- start
                changed.Validate();
 diff --- end */
/* diff +++ start */
                foreach (var v in changed) {
                    v.Validate();
                }
/* diff +++ end */
                return true;
            }
            catch (Gs2Exception) {
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
            DeleteEntriesByUserIdRequest request
        ) {
/* diff --- start
            return null;
 diff --- end */
/* diff +++ start */
            var items = self.ToList();
            foreach (var entryModelName in request.EntryModelNames) {
                if (!items.Select(v => v.Name).ToList().Contains(entryModelName)) {
                    items.Add(new Entry {
                        UserId = request.UserId,
                        Name = entryModelName,
                        AcquiredAt = UnixTime.ToUnixTime(DateTime.Now),
                    });
                }
            }
            return items.ToArray();
/* diff +++ end */
        }

        public static DeleteEntriesByUserIdRequest Rate(
            this DeleteEntriesByUserIdRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:DeleteEntriesByUserId");
        }
    }

    public static partial class DeleteEntriesByUserIdRequestExt
    {
        public static DeleteEntriesByUserIdRequest Rate(
            this DeleteEntriesByUserIdRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Dictionary:DeleteEntriesByUserId");
        }
    }
}