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
using Gs2.Gs2Grade.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Grade.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateGradeModelMasterRequest : Gs2Request<UpdateGradeModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string GradeName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public Gs2.Gs2Grade.Model.DefaultGradeModel[] DefaultGrades { set; get; } = null!;
         public string ExperienceModelId { set; get; } = null!;
         public Gs2.Gs2Grade.Model.GradeEntryModel[] GradeEntries { set; get; } = null!;
         public Gs2.Gs2Grade.Model.AcquireActionRate[] AcquireActionRates { set; get; } = null!;
        public UpdateGradeModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateGradeModelMasterRequest WithGradeName(string gradeName) {
            this.GradeName = gradeName;
            return this;
        }
        public UpdateGradeModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateGradeModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateGradeModelMasterRequest WithDefaultGrades(Gs2.Gs2Grade.Model.DefaultGradeModel[] defaultGrades) {
            this.DefaultGrades = defaultGrades;
            return this;
        }
        public UpdateGradeModelMasterRequest WithExperienceModelId(string experienceModelId) {
            this.ExperienceModelId = experienceModelId;
            return this;
        }
        public UpdateGradeModelMasterRequest WithGradeEntries(Gs2.Gs2Grade.Model.GradeEntryModel[] gradeEntries) {
            this.GradeEntries = gradeEntries;
            return this;
        }
        public UpdateGradeModelMasterRequest WithAcquireActionRates(Gs2.Gs2Grade.Model.AcquireActionRate[] acquireActionRates) {
            this.AcquireActionRates = acquireActionRates;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateGradeModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateGradeModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGradeName(!data.Keys.Contains("gradeName") || data["gradeName"] == null ? null : data["gradeName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithDefaultGrades(!data.Keys.Contains("defaultGrades") || data["defaultGrades"] == null || !data["defaultGrades"].IsArray ? null : data["defaultGrades"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.DefaultGradeModel.FromJson(v);
                }).ToArray())
                .WithExperienceModelId(!data.Keys.Contains("experienceModelId") || data["experienceModelId"] == null ? null : data["experienceModelId"].ToString())
                .WithGradeEntries(!data.Keys.Contains("gradeEntries") || data["gradeEntries"] == null || !data["gradeEntries"].IsArray ? null : data["gradeEntries"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.GradeEntryModel.FromJson(v);
                }).ToArray())
                .WithAcquireActionRates(!data.Keys.Contains("acquireActionRates") || data["acquireActionRates"] == null || !data["acquireActionRates"].IsArray ? null : data["acquireActionRates"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Grade.Model.AcquireActionRate.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData defaultGradesJsonData = null;
            if (DefaultGrades != null && DefaultGrades.Length > 0)
            {
                defaultGradesJsonData = new JsonData();
                foreach (var defaultGrade in DefaultGrades)
                {
                    defaultGradesJsonData.Add(defaultGrade.ToJson());
                }
            }
            JsonData gradeEntriesJsonData = null;
            if (GradeEntries != null && GradeEntries.Length > 0)
            {
                gradeEntriesJsonData = new JsonData();
                foreach (var gradeEntry in GradeEntries)
                {
                    gradeEntriesJsonData.Add(gradeEntry.ToJson());
                }
            }
            JsonData acquireActionRatesJsonData = null;
            if (AcquireActionRates != null && AcquireActionRates.Length > 0)
            {
                acquireActionRatesJsonData = new JsonData();
                foreach (var acquireActionRate in AcquireActionRates)
                {
                    acquireActionRatesJsonData.Add(acquireActionRate.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["gradeName"] = GradeName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["defaultGrades"] = defaultGradesJsonData,
                ["experienceModelId"] = ExperienceModelId,
                ["gradeEntries"] = gradeEntriesJsonData,
                ["acquireActionRates"] = acquireActionRatesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (GradeName != null) {
                writer.WritePropertyName("gradeName");
                writer.Write(GradeName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (DefaultGrades != null) {
                writer.WritePropertyName("defaultGrades");
                writer.WriteArrayStart();
                foreach (var defaultGrade in DefaultGrades)
                {
                    if (defaultGrade != null) {
                        defaultGrade.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ExperienceModelId != null) {
                writer.WritePropertyName("experienceModelId");
                writer.Write(ExperienceModelId.ToString());
            }
            if (GradeEntries != null) {
                writer.WritePropertyName("gradeEntries");
                writer.WriteArrayStart();
                foreach (var gradeEntry in GradeEntries)
                {
                    if (gradeEntry != null) {
                        gradeEntry.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActionRates != null) {
                writer.WritePropertyName("acquireActionRates");
                writer.WriteArrayStart();
                foreach (var acquireActionRate in AcquireActionRates)
                {
                    if (acquireActionRate != null) {
                        acquireActionRate.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += GradeName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += DefaultGrades + ":";
            key += ExperienceModelId + ":";
            key += GradeEntries + ":";
            key += AcquireActionRates + ":";
            return key;
        }
    }
}