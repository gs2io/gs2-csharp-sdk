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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Version.Model
{

	[Preserve]
	public class Status : IComparable
	{
        public Gs2.Gs2Version.Model.VersionModel VersionModel { set; get; }
        public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; }

        public Status WithVersionModel(Gs2.Gs2Version.Model.VersionModel versionModel) {
            this.VersionModel = versionModel;
            return this;
        }

        public Status WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
            return this;
        }

    	[Preserve]
        public static Status FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Status()
                .WithVersionModel(!data.Keys.Contains("versionModel") || data["versionModel"] == null ? null : Gs2.Gs2Version.Model.VersionModel.FromJson(data["versionModel"]))
                .WithCurrentVersion(!data.Keys.Contains("currentVersion") || data["currentVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["currentVersion"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["versionModel"] = VersionModel?.ToJson(),
                ["currentVersion"] = CurrentVersion?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (VersionModel != null) {
                writer.WritePropertyName("versionModel");
                VersionModel.WriteJson(writer);
            }
            if (CurrentVersion != null) {
                writer.WritePropertyName("currentVersion");
                CurrentVersion.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Status;
            var diff = 0;
            if (VersionModel == null && VersionModel == other.VersionModel)
            {
                // null and null
            }
            else
            {
                diff += VersionModel.CompareTo(other.VersionModel);
            }
            if (CurrentVersion == null && CurrentVersion == other.CurrentVersion)
            {
                // null and null
            }
            else
            {
                diff += CurrentVersion.CompareTo(other.CurrentVersion);
            }
            return diff;
        }
    }
}