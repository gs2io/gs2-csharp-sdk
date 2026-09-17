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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Status : IComparable
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Status FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Status.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(VersionModel, other.VersionModel);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CurrentVersion, other.CurrentVersion);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
            }
            {
            }
        }

        public object Clone() {
            return new Status {
                VersionModel = VersionModel?.Clone() as Gs2.Gs2Version.Model.VersionModel,
                CurrentVersion = CurrentVersion?.Clone() as Gs2.Gs2Version.Model.Version_,
            };
        }
    }
}