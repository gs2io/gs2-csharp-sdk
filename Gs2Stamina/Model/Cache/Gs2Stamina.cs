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

using Gs2.Core.Domain;

namespace Gs2.Gs2Stamina.Model.Cache
{
    public static class Gs2Stamina
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeNamespaces":
                    Result.DescribeNamespacesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeNamespacesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNamespace":
                    Result.CreateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespaceStatus":
                    Result.GetNamespaceStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNamespaceStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespace":
                    Result.GetNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNamespace":
                    Result.UpdateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNamespace":
                    Result.DeleteNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "dumpUserDataByUserId":
                    Result.DumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkDumpUserDataByUserId":
                    Result.CheckDumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckDumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserDataByUserId":
                    Result.CleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkCleanUserDataByUserId":
                    Result.CheckCleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckCleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserDataByUserId":
                    Result.PrepareImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.PrepareImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserDataByUserId":
                    Result.ImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkImportUserDataByUserId":
                    Result.CheckImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CheckImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStaminaModelMasters":
                    Result.DescribeStaminaModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStaminaModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createStaminaModelMaster":
                    Result.CreateStaminaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateStaminaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStaminaModelMaster":
                    Result.GetStaminaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStaminaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStaminaModelMaster":
                    Result.UpdateStaminaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateStaminaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStaminaModelMaster":
                    Result.DeleteStaminaModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStaminaModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMaxStaminaTableMasters":
                    Result.DescribeMaxStaminaTableMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMaxStaminaTableMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMaxStaminaTableMaster":
                    Result.CreateMaxStaminaTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateMaxStaminaTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMaxStaminaTableMaster":
                    Result.GetMaxStaminaTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMaxStaminaTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMaxStaminaTableMaster":
                    Result.UpdateMaxStaminaTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMaxStaminaTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMaxStaminaTableMaster":
                    Result.DeleteMaxStaminaTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMaxStaminaTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRecoverIntervalTableMasters":
                    Result.DescribeRecoverIntervalTableMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRecoverIntervalTableMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRecoverIntervalTableMaster":
                    Result.CreateRecoverIntervalTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRecoverIntervalTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRecoverIntervalTableMaster":
                    Result.GetRecoverIntervalTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRecoverIntervalTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRecoverIntervalTableMaster":
                    Result.UpdateRecoverIntervalTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRecoverIntervalTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRecoverIntervalTableMaster":
                    Result.DeleteRecoverIntervalTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRecoverIntervalTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeRecoverValueTableMasters":
                    Result.DescribeRecoverValueTableMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeRecoverValueTableMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createRecoverValueTableMaster":
                    Result.CreateRecoverValueTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateRecoverValueTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getRecoverValueTableMaster":
                    Result.GetRecoverValueTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetRecoverValueTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateRecoverValueTableMaster":
                    Result.UpdateRecoverValueTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateRecoverValueTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteRecoverValueTableMaster":
                    Result.DeleteRecoverValueTableMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteRecoverValueTableMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentStaminaMaster":
                    Result.GetCurrentStaminaMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentStaminaMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentStaminaMaster":
                    Result.UpdateCurrentStaminaMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentStaminaMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentStaminaMasterFromGitHub":
                    Result.UpdateCurrentStaminaMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentStaminaMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStaminaModels":
                    Result.DescribeStaminaModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStaminaModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStaminaModel":
                    Result.GetStaminaModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStaminaModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStaminas":
                    Result.DescribeStaminasResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStaminasRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeStaminasByUserId":
                    Result.DescribeStaminasByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeStaminasByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStamina":
                    Result.GetStaminaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStaminaRequest.FromJson(requestPayload)
                    );
                    break;
                case "getStaminaByUserId":
                    Result.GetStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateStaminaByUserId":
                    Result.UpdateStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeStamina":
                    Result.ConsumeStaminaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ConsumeStaminaRequest.FromJson(requestPayload)
                    );
                    break;
                case "consumeStaminaByUserId":
                    Result.ConsumeStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ConsumeStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyStamina":
                    Result.ApplyStaminaResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ApplyStaminaRequest.FromJson(requestPayload)
                    );
                    break;
                case "applyStaminaByUserId":
                    Result.ApplyStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ApplyStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "recoverStaminaByUserId":
                    Result.RecoverStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RecoverStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "raiseMaxValueByUserId":
                    Result.RaiseMaxValueByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.RaiseMaxValueByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaxValue":
                    Result.DecreaseMaxValueResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaxValueRequest.FromJson(requestPayload)
                    );
                    break;
                case "decreaseMaxValueByUserId":
                    Result.DecreaseMaxValueByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DecreaseMaxValueByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMaxValueByUserId":
                    Result.SetMaxValueByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetMaxValueByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRecoverIntervalByUserId":
                    Result.SetRecoverIntervalByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRecoverIntervalByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRecoverValueByUserId":
                    Result.SetRecoverValueByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRecoverValueByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMaxValueByStatus":
                    Result.SetMaxValueByStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetMaxValueByStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRecoverIntervalByStatus":
                    Result.SetRecoverIntervalByStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRecoverIntervalByStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "setRecoverValueByStatus":
                    Result.SetRecoverValueByStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetRecoverValueByStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteStaminaByUserId":
                    Result.DeleteStaminaByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteStaminaByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}