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

namespace Gs2.Gs2Formation.Model.Cache
{
    public static class Gs2Formation
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
                case "getFormModel":
                    Result.GetFormModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFormModelMasters":
                    Result.DescribeFormModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFormModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createFormModelMaster":
                    Result.CreateFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormModelMaster":
                    Result.GetFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateFormModelMaster":
                    Result.UpdateFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFormModelMaster":
                    Result.DeleteFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldModels":
                    Result.DescribeMoldModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMoldModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldModel":
                    Result.GetMoldModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMoldModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldModelMasters":
                    Result.DescribeMoldModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMoldModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMoldModelMaster":
                    Result.CreateMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreateMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldModelMaster":
                    Result.GetMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMoldModelMaster":
                    Result.UpdateMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMoldModelMaster":
                    Result.DeleteMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormModels":
                    Result.DescribePropertyFormModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePropertyFormModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormModel":
                    Result.GetPropertyFormModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormModelMasters":
                    Result.DescribePropertyFormModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePropertyFormModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPropertyFormModelMaster":
                    Result.CreatePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.CreatePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormModelMaster":
                    Result.GetPropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updatePropertyFormModelMaster":
                    Result.UpdatePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdatePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyFormModelMaster":
                    Result.DeletePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentFormMaster":
                    Result.GetCurrentFormMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetCurrentFormMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFormMaster":
                    Result.UpdateCurrentFormMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentFormMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFormMasterFromGitHub":
                    Result.UpdateCurrentFormMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.UpdateCurrentFormMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMolds":
                    Result.DescribeMoldsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMoldsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldsByUserId":
                    Result.DescribeMoldsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeMoldsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMold":
                    Result.GetMoldResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMoldRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldByUserId":
                    Result.GetMoldByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetMoldByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMoldCapacityByUserId":
                    Result.SetMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addMoldCapacityByUserId":
                    Result.AddMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AddMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "subMoldCapacity":
                    Result.SubMoldCapacityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubMoldCapacityRequest.FromJson(requestPayload)
                    );
                    break;
                case "subMoldCapacityByUserId":
                    Result.SubMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SubMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMold":
                    Result.DeleteMoldResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMoldRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMoldByUserId":
                    Result.DeleteMoldByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteMoldByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeForms":
                    Result.DescribeFormsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFormsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFormsByUserId":
                    Result.DescribeFormsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribeFormsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getForm":
                    Result.GetFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormByUserId":
                    Result.GetFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormWithSignature":
                    Result.GetFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormWithSignatureByUserId":
                    Result.GetFormWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetFormWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setForm":
                    Result.SetFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFormByUserId":
                    Result.SetFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFormWithSignature":
                    Result.SetFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireActionsToFormProperties":
                    Result.AcquireActionsToFormPropertiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcquireActionsToFormPropertiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteForm":
                    Result.DeleteFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFormByUserId":
                    Result.DeleteFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeleteFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyForms":
                    Result.DescribePropertyFormsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePropertyFormsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormsByUserId":
                    Result.DescribePropertyFormsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DescribePropertyFormsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyForm":
                    Result.GetPropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormByUserId":
                    Result.GetPropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormWithSignature":
                    Result.GetPropertyFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormWithSignatureByUserId":
                    Result.GetPropertyFormWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.GetPropertyFormWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyForm":
                    Result.SetPropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetPropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyFormByUserId":
                    Result.SetPropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetPropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyFormWithSignature":
                    Result.SetPropertyFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.SetPropertyFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireActionsToPropertyFormProperties":
                    Result.AcquireActionsToPropertyFormPropertiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.AcquireActionsToPropertyFormPropertiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyForm":
                    Result.DeletePropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyFormByUserId":
                    Result.DeletePropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        Request.DeletePropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}