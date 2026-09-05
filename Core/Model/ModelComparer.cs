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

namespace Gs2.Core.Model
{
    internal static class ModelComparer
    {
        internal static int Compare<T>(T left, T right)
        {
            return Comparer<T>.Default.Compare(left, right);
        }

        internal static int CompareArray<T>(T[] left, T[] right)
        {
            if (ReferenceEquals(left, right))
            {
                return 0;
            }
            if (ReferenceEquals(left, null))
            {
                return -1;
            }
            if (ReferenceEquals(right, null))
            {
                return 1;
            }

            var length = Math.Min(left.Length, right.Length);
            for (var i = 0; i < length; i++)
            {
                var diff = Compare(left[i], right[i]);
                if (diff != 0)
                {
                    return diff;
                }
            }
            return left.Length.CompareTo(right.Length);
        }
    }
}
