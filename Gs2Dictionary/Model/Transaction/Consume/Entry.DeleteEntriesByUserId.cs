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
            DeleteEntriesByUserIdRequest request
        ) {
/* diff --- start
            return null;
 diff --- end */
/* diff +++ start */
            if (self == null ||
                request == null ||
                self.Any(v => v?.Name == null)) {
                throw new NullReferenceException();
            }
            var entryModelNames = (request.EntryModelNames ?? Array.Empty<string>()).ToHashSet();
            return self.Where(v => !entryModelNames.Contains(v.Name)).ToArray();
/* diff +++ end */
        }

        public static DeleteEntriesByUserIdRequest Rate(
            this DeleteEntriesByUserIdRequest request,
            double rate
        ) {
            return request;
        }
    }

    public static partial class DeleteEntriesByUserIdRequestExt
    {
        public static DeleteEntriesByUserIdRequest Rate(
            this DeleteEntriesByUserIdRequest request,
            BigInteger rate
        ) {
            return request;
        }
    }
}
