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

namespace Gs2.Gs2Grade.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ApplyRankCapByUserIdResult : IResult
	{
        public Gs2.Gs2Grade.Model.Status Item { set; get; }
        public string ExperienceNamespaceName { set; get; }
        public Gs2.Gs2Experience.Model.Status ExperienceStatus { set; get; }

        public ApplyRankCapByUserIdResult WithItem(Gs2.Gs2Grade.Model.Status item) {
            this.Item = item;
            return this;
        }

        public ApplyRankCapByUserIdResult WithExperienceNamespaceName(string experienceNamespaceName) {
            this.ExperienceNamespaceName = experienceNamespaceName;
            return this;
        }

        public ApplyRankCapByUserIdResult WithExperienceStatus(Gs2.Gs2Experience.Model.Status experienceStatus) {
            this.ExperienceStatus = experienceStatus;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ApplyRankCapByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ApplyRankCapByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Grade.Model.Status.FromJson(data["item"]))
                .WithExperienceNamespaceName(!data.Keys.Contains("experienceNamespaceName") || data["experienceNamespaceName"] == null ? null : data["experienceNamespaceName"].ToString())
                .WithExperienceStatus(!data.Keys.Contains("experienceStatus") || data["experienceStatus"] == null ? null : Gs2.Gs2Experience.Model.Status.FromJson(data["experienceStatus"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["experienceNamespaceName"] = ExperienceNamespaceName,
                ["experienceStatus"] = ExperienceStatus?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (ExperienceNamespaceName != null) {
                writer.WritePropertyName("experienceNamespaceName");
                writer.Write(ExperienceNamespaceName.ToString());
            }
            if (ExperienceStatus != null) {
                ExperienceStatus.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}