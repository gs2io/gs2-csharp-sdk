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
                        null,
                        Request.DescribeNamespacesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNamespace":
                    Result.CreateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespaceStatus":
                    Result.GetNamespaceStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespace":
                    Result.GetNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNamespace":
                    Result.UpdateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNamespace":
                    Result.DeleteNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "dumpUserDataByUserId":
                    Result.DumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkDumpUserDataByUserId":
                    Result.CheckDumpUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckDumpUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "cleanUserDataByUserId":
                    Result.CleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkCleanUserDataByUserId":
                    Result.CheckCleanUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckCleanUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "prepareImportUserDataByUserId":
                    Result.PrepareImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PrepareImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "importUserDataByUserId":
                    Result.ImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "checkImportUserDataByUserId":
                    Result.CheckImportUserDataByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CheckImportUserDataByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormModel":
                    Result.GetFormModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFormModelMasters":
                    Result.DescribeFormModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeFormModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createFormModelMaster":
                    Result.CreateFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormModelMaster":
                    Result.GetFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateFormModelMaster":
                    Result.UpdateFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFormModelMaster":
                    Result.DeleteFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldModels":
                    Result.DescribeMoldModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMoldModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldModel":
                    Result.GetMoldModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMoldModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldModelMasters":
                    Result.DescribeMoldModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMoldModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createMoldModelMaster":
                    Result.CreateMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldModelMaster":
                    Result.GetMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateMoldModelMaster":
                    Result.UpdateMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMoldModelMaster":
                    Result.DeleteMoldModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMoldModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormModels":
                    Result.DescribePropertyFormModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePropertyFormModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormModel":
                    Result.GetPropertyFormModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormModelMasters":
                    Result.DescribePropertyFormModelMastersResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePropertyFormModelMastersRequest.FromJson(requestPayload)
                    );
                    break;
                case "createPropertyFormModelMaster":
                    Result.CreatePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreatePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormModelMaster":
                    Result.GetPropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updatePropertyFormModelMaster":
                    Result.UpdatePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdatePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyFormModelMaster":
                    Result.DeletePropertyFormModelMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePropertyFormModelMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "exportMaster":
                    Result.ExportMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.ExportMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "getCurrentFormMaster":
                    Result.GetCurrentFormMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetCurrentFormMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "preUpdateCurrentFormMaster":
                    Result.PreUpdateCurrentFormMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.PreUpdateCurrentFormMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFormMaster":
                    Result.UpdateCurrentFormMasterResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentFormMasterRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateCurrentFormMasterFromGitHub":
                    Result.UpdateCurrentFormMasterFromGitHubResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateCurrentFormMasterFromGitHubRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMolds":
                    Result.DescribeMoldsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMoldsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMoldsByUserId":
                    Result.DescribeMoldsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMoldsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMold":
                    Result.GetMoldResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMoldRequest.FromJson(requestPayload)
                    );
                    break;
                case "getMoldByUserId":
                    Result.GetMoldByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetMoldByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setMoldCapacityByUserId":
                    Result.SetMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "addMoldCapacityByUserId":
                    Result.AddMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AddMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "subMoldCapacity":
                    Result.SubMoldCapacityResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SubMoldCapacityRequest.FromJson(requestPayload)
                    );
                    break;
                case "subMoldCapacityByUserId":
                    Result.SubMoldCapacityByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SubMoldCapacityByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMold":
                    Result.DeleteMoldResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMoldRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteMoldByUserId":
                    Result.DeleteMoldByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteMoldByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeForms":
                    Result.DescribeFormsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeFormsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFormsByUserId":
                    Result.DescribeFormsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeFormsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getForm":
                    Result.GetFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormByUserId":
                    Result.GetFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormWithSignature":
                    Result.GetFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFormWithSignatureByUserId":
                    Result.GetFormWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFormWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setForm":
                    Result.SetFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFormByUserId":
                    Result.SetFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setFormWithSignature":
                    Result.SetFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireActionsToFormProperties":
                    Result.AcquireActionsToFormPropertiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireActionsToFormPropertiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteForm":
                    Result.DeleteFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFormByUserId":
                    Result.DeleteFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyForms":
                    Result.DescribePropertyFormsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePropertyFormsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describePropertyFormsByUserId":
                    Result.DescribePropertyFormsByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribePropertyFormsByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyForm":
                    Result.GetPropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormByUserId":
                    Result.GetPropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormWithSignature":
                    Result.GetPropertyFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "getPropertyFormWithSignatureByUserId":
                    Result.GetPropertyFormWithSignatureByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetPropertyFormWithSignatureByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyForm":
                    Result.SetPropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetPropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyFormByUserId":
                    Result.SetPropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetPropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "setPropertyFormWithSignature":
                    Result.SetPropertyFormWithSignatureResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SetPropertyFormWithSignatureRequest.FromJson(requestPayload)
                    );
                    break;
                case "acquireActionsToPropertyFormProperties":
                    Result.AcquireActionsToPropertyFormPropertiesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.AcquireActionsToPropertyFormPropertiesRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyForm":
                    Result.DeletePropertyFormResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePropertyFormRequest.FromJson(requestPayload)
                    );
                    break;
                case "deletePropertyFormByUserId":
                    Result.DeletePropertyFormByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeletePropertyFormByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}