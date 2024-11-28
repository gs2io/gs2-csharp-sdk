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
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Version.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateVersionModelMasterRequest : Gs2Request<CreateVersionModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public string Scope { set; get; } = null!;
         public string Type { set; get; } = null!;
         public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; } = null!;
         public Gs2.Gs2Version.Model.Version_ WarningVersion { set; get; } = null!;
         public Gs2.Gs2Version.Model.Version_ ErrorVersion { set; get; } = null!;
         public Gs2.Gs2Version.Model.ScheduleVersion[] ScheduleVersions { set; get; } = null!;
         public bool? NeedSignature { set; get; } = null!;
         public string SignatureKeyId { set; get; } = null!;
        public CreateVersionModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CreateVersionModelMasterRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateVersionModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateVersionModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public CreateVersionModelMasterRequest WithScope(string scope) {
            this.Scope = scope;
            return this;
        }
        public CreateVersionModelMasterRequest WithType(string type) {
            this.Type = type;
            return this;
        }
        public CreateVersionModelMasterRequest WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
            return this;
        }
        public CreateVersionModelMasterRequest WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.WarningVersion = warningVersion;
            return this;
        }
        public CreateVersionModelMasterRequest WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.ErrorVersion = errorVersion;
            return this;
        }
        public CreateVersionModelMasterRequest WithScheduleVersions(Gs2.Gs2Version.Model.ScheduleVersion[] scheduleVersions) {
            this.ScheduleVersions = scheduleVersions;
            return this;
        }
        public CreateVersionModelMasterRequest WithNeedSignature(bool? needSignature) {
            this.NeedSignature = needSignature;
            return this;
        }
        public CreateVersionModelMasterRequest WithSignatureKeyId(string signatureKeyId) {
            this.SignatureKeyId = signatureKeyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateVersionModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateVersionModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScope(!data.Keys.Contains("scope") || data["scope"] == null ? null : data["scope"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithCurrentVersion(!data.Keys.Contains("currentVersion") || data["currentVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["currentVersion"]))
                .WithWarningVersion(!data.Keys.Contains("warningVersion") || data["warningVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["warningVersion"]))
                .WithErrorVersion(!data.Keys.Contains("errorVersion") || data["errorVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["errorVersion"]))
                .WithScheduleVersions(!data.Keys.Contains("scheduleVersions") || data["scheduleVersions"] == null || !data["scheduleVersions"].IsArray ? null : data["scheduleVersions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Version.Model.ScheduleVersion.FromJson(v);
                }).ToArray())
                .WithNeedSignature(!data.Keys.Contains("needSignature") || data["needSignature"] == null ? null : (bool?)bool.Parse(data["needSignature"].ToString()))
                .WithSignatureKeyId(!data.Keys.Contains("signatureKeyId") || data["signatureKeyId"] == null ? null : data["signatureKeyId"].ToString());
        }

        public override JsonData ToJson()
        {
            JsonData scheduleVersionsJsonData = null;
            if (ScheduleVersions != null && ScheduleVersions.Length > 0)
            {
                scheduleVersionsJsonData = new JsonData();
                foreach (var scheduleVersion in ScheduleVersions)
                {
                    scheduleVersionsJsonData.Add(scheduleVersion.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["scope"] = Scope,
                ["type"] = Type,
                ["currentVersion"] = CurrentVersion?.ToJson(),
                ["warningVersion"] = WarningVersion?.ToJson(),
                ["errorVersion"] = ErrorVersion?.ToJson(),
                ["scheduleVersions"] = scheduleVersionsJsonData,
                ["needSignature"] = NeedSignature,
                ["signatureKeyId"] = SignatureKeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Scope != null) {
                writer.WritePropertyName("scope");
                writer.Write(Scope.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (CurrentVersion != null) {
                CurrentVersion.WriteJson(writer);
            }
            if (WarningVersion != null) {
                WarningVersion.WriteJson(writer);
            }
            if (ErrorVersion != null) {
                ErrorVersion.WriteJson(writer);
            }
            if (ScheduleVersions != null) {
                writer.WritePropertyName("scheduleVersions");
                writer.WriteArrayStart();
                foreach (var scheduleVersion in ScheduleVersions)
                {
                    if (scheduleVersion != null) {
                        scheduleVersion.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (NeedSignature != null) {
                writer.WritePropertyName("needSignature");
                writer.Write(bool.Parse(NeedSignature.ToString()));
            }
            if (SignatureKeyId != null) {
                writer.WritePropertyName("signatureKeyId");
                writer.Write(SignatureKeyId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += Name + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += Scope + ":";
            key += Type + ":";
            key += CurrentVersion + ":";
            key += WarningVersion + ":";
            key += ErrorVersion + ":";
            key += ScheduleVersions + ":";
            key += NeedSignature + ":";
            key += SignatureKeyId + ":";
            return key;
        }
    }
}