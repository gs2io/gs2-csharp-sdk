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
using System.Linq;
using System.Numerics;
using Gs2.Core.Exception;
using Gs2.Gs2Script.Request;

namespace Gs2.Gs2Script.Model.Transaction
{
    public static partial class ScriptExt
    {
        public static bool IsExecutable(
            this Script self,
            InvokeScriptRequest request
        ) {
            var changed = self.SpeculativeExecution(request);
            try {
                changed.Validate();
                return true;
            }
            catch (Gs2Exception) {
                return false;
            }
        }

        public static Script SpeculativeExecution(
            this Script self,
            InvokeScriptRequest request
        ) {
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning("Speculative execution not supported on this action: Gs2Script:InvokeScript");
#else
            System.Console.WriteLine("Speculative execution not supported on this action: Gs2Script:InvokeScript");
#endif
            return self.Clone() as Script;
        }

        public static InvokeScriptRequest Rate(
            this InvokeScriptRequest request,
            double rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Script:InvokeScript");
        }
    }

    public static partial class InvokeScriptRequestExt
    {
        public static InvokeScriptRequest Rate(
            this InvokeScriptRequest request,
            BigInteger rate
        ) {
            throw new NotSupportedException($"not supported rate action Gs2Script:InvokeScript");
        }
    }
}