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

using System.Collections.Generic;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Distributor.Model;
using Gs2.Gs2JobQueue.Model;
#if UNITY_2017_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Profiling;
#endif

namespace Gs2.Core.Util
{
    public static class Telemetry
    {
        public delegate void StartRequestHandler(Gs2SessionTaskId taskId, IRequest request);
        public static event StartRequestHandler OnStartRequest;
        
        public delegate void EndRequestHandler(Gs2SessionTaskId taskId, IRequest request, IGs2SessionResult result);
        public static event EndRequestHandler OnEndRequest;
        
        public delegate void StartTransactionHandler(string transactionId, IRequest request);
        public static event StartTransactionHandler OnStartTransaction;
        
        public delegate void EndTransactionHandler(string transactionId);
        public static event EndTransactionHandler OnEndTransaction;

        public delegate void HandleTransactionHandler(string transactionId, StampSheetResult result);
        public static event HandleTransactionHandler OnHandleTransaction;

        public delegate void StartJobHandler(string jobName);
        public static event StartJobHandler OnStartJob;
        
        public delegate void EndJobHandler(string jobName);
        public static event EndJobHandler OnEndJob;

        public delegate void HandleJobHandler(string jobName, JobResult result);
        public static event HandleJobHandler OnHandleJob;

        internal static void StartRequest(Gs2SessionTaskId taskId, IRequest request) {
            OnStartRequest?.Invoke(taskId, request);
        }
        
        internal static void EndRequest(Gs2SessionTaskId taskId, IRequest request, IGs2SessionResult result) {
            OnEndRequest?.Invoke(taskId, request, result);
        }
        
        internal static void StartTransaction(string transactionId, IRequest request) {
            OnStartTransaction?.Invoke(transactionId, request);
        }
        
        internal static void EndTransaction(string transactionId) {
            OnEndTransaction?.Invoke(transactionId);
        }
        
        internal static void HandleTransaction(string transactionId, StampSheetResult result) {
            OnHandleTransaction?.Invoke(transactionId, result);
        }
        
        internal static void StartJob(string jobName) {
            OnStartJob?.Invoke(jobName);
        }
        
        internal static void EndJob(string jobName) {
            OnEndJob?.Invoke(jobName);
        }
        
        internal static void HandleJob(string jobName, JobResult result) {
            OnHandleJob?.Invoke(jobName, result);
        }
    }
}
