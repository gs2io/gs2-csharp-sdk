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
using System.Collections;
using System.Linq;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2JobQueue.Model;
using Gs2.Gs2JobQueue.Request;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER 
using UnityEngine;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #endif
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public static class JobQueueJobDomainFactory
    {
        public static TransactionAccessTokenDomain ToTransaction(
            Gs2 gs2,
            AccessToken accessToken,
            PushByUserIdResult result
        ) {
            if (result.AutoRun ?? false) {
                return new TransactionAccessTokenDomain(
                    gs2,
                    accessToken,
                    result.Items.Select(v => new AutoJobQueueAccessTokenDomain(
                        gs2,
                        accessToken,
                        Job.GetNamespaceNameFromGrn(v.JobId),
                        Job.GetJobNameFromGrn(v.JobId)
                    )).ToList<TransactionAccessTokenDomain>()
                );
            }
            else {
                foreach (var job in result.Items) {
                    gs2.PushJobQueue(Job.GetNamespaceNameFromGrn(job.JobId));
                }
                return new TransactionAccessTokenDomain(
                    gs2,
                    accessToken,
                    result.Items.Select(v => new ManualJobQueueAccessTokenDomain(
                        gs2,
                        accessToken,
                        Job.GetNamespaceNameFromGrn(v.JobId),
                        Job.GetJobNameFromGrn(v.JobId)
                    )).ToList<TransactionAccessTokenDomain>()
                );
            }
        }
        
        public static TransactionDomain ToTransaction(
            Gs2 gs2,
            string userId,
            PushByUserIdResult result
        ) {
            if (result.AutoRun ?? false) {
                return new TransactionDomain(
                    gs2,
                    userId,
                    result.Items.Select(v => new AutoJobQueueDomain(
                        gs2,
                        userId,
                        Job.GetNamespaceNameFromGrn(v.JobId),
                        Job.GetJobNameFromGrn(v.JobId)
                    )).ToList<TransactionDomain>()
                );
            }
            else {
                foreach (var job in result.Items) {
                    gs2.PushJobQueue(Job.GetNamespaceNameFromGrn(job.JobId));
                }
                return new TransactionDomain(
                    gs2,
                    userId,
                    result.Items.Select(v => new ManualJobQueueDomain(
                        gs2,
                        userId,
                        Job.GetNamespaceNameFromGrn(v.JobId),
                        Job.GetJobNameFromGrn(v.JobId)
                    )).ToList<TransactionDomain>()
                );
            }
        }
    }
}
