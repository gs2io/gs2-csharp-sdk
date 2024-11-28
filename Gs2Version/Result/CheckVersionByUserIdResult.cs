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

namespace Gs2.Gs2Version.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CheckVersionByUserIdResult : IResult
	{
        public string ProjectToken { set; get; } = null!;
        public Gs2.Gs2Version.Model.Status[] Warnings { set; get; } = null!;
        public Gs2.Gs2Version.Model.Status[] Errors { set; get; } = null!;

        public CheckVersionByUserIdResult WithProjectToken(string projectToken) {
            this.ProjectToken = projectToken;
            return this;
        }

        public CheckVersionByUserIdResult WithWarnings(Gs2.Gs2Version.Model.Status[] warnings) {
            this.Warnings = warnings;
            return this;
        }

        public CheckVersionByUserIdResult WithErrors(Gs2.Gs2Version.Model.Status[] errors) {
            this.Errors = errors;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CheckVersionByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CheckVersionByUserIdResult()
                .WithProjectToken(!data.Keys.Contains("projectToken") || data["projectToken"] == null ? null : data["projectToken"].ToString())
                .WithWarnings(!data.Keys.Contains("warnings") || data["warnings"] == null || !data["warnings"].IsArray ? null : data["warnings"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Version.Model.Status.FromJson(v);
                }).ToArray())
                .WithErrors(!data.Keys.Contains("errors") || data["errors"] == null || !data["errors"].IsArray ? null : data["errors"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Version.Model.Status.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData warningsJsonData = null;
            if (Warnings != null && Warnings.Length > 0)
            {
                warningsJsonData = new JsonData();
                foreach (var warning in Warnings)
                {
                    warningsJsonData.Add(warning.ToJson());
                }
            }
            JsonData errorsJsonData = null;
            if (Errors != null && Errors.Length > 0)
            {
                errorsJsonData = new JsonData();
                foreach (var error in Errors)
                {
                    errorsJsonData.Add(error.ToJson());
                }
            }
            return new JsonData {
                ["projectToken"] = ProjectToken,
                ["warnings"] = warningsJsonData,
                ["errors"] = errorsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ProjectToken != null) {
                writer.WritePropertyName("projectToken");
                writer.Write(ProjectToken.ToString());
            }
            if (Warnings != null) {
                writer.WritePropertyName("warnings");
                writer.WriteArrayStart();
                foreach (var warning in Warnings)
                {
                    if (warning != null) {
                        warning.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Errors != null) {
                writer.WritePropertyName("errors");
                writer.WriteArrayStart();
                foreach (var error in Errors)
                {
                    if (error != null) {
                        error.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }
    }
}