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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Version.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ScheduleVersion : IComparable
	{
        public Gs2.Gs2Version.Model.Version_ CurrentVersion { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ WarningVersion { set; get; } = null!;
        public Gs2.Gs2Version.Model.Version_ ErrorVersion { set; get; } = null!;
        public string ScheduleEventId { set; get; } = null!;
        public ScheduleVersion WithCurrentVersion(Gs2.Gs2Version.Model.Version_ currentVersion) {
            this.CurrentVersion = currentVersion;
            return this;
        }
        public ScheduleVersion WithWarningVersion(Gs2.Gs2Version.Model.Version_ warningVersion) {
            this.WarningVersion = warningVersion;
            return this;
        }
        public ScheduleVersion WithErrorVersion(Gs2.Gs2Version.Model.Version_ errorVersion) {
            this.ErrorVersion = errorVersion;
            return this;
        }
        public ScheduleVersion WithScheduleEventId(string scheduleEventId) {
            this.ScheduleEventId = scheduleEventId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ScheduleVersion FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ScheduleVersion()
                .WithCurrentVersion(!data.Keys.Contains("currentVersion") || data["currentVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["currentVersion"]))
                .WithWarningVersion(!data.Keys.Contains("warningVersion") || data["warningVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["warningVersion"]))
                .WithErrorVersion(!data.Keys.Contains("errorVersion") || data["errorVersion"] == null ? null : Gs2.Gs2Version.Model.Version_.FromJson(data["errorVersion"]))
                .WithScheduleEventId(!data.Keys.Contains("scheduleEventId") || data["scheduleEventId"] == null ? null : data["scheduleEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["currentVersion"] = CurrentVersion?.ToJson(),
                ["warningVersion"] = WarningVersion?.ToJson(),
                ["errorVersion"] = ErrorVersion?.ToJson(),
                ["scheduleEventId"] = ScheduleEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CurrentVersion != null) {
                writer.WritePropertyName("currentVersion");
                CurrentVersion.WriteJson(writer);
            }
            if (WarningVersion != null) {
                writer.WritePropertyName("warningVersion");
                WarningVersion.WriteJson(writer);
            }
            if (ErrorVersion != null) {
                writer.WritePropertyName("errorVersion");
                ErrorVersion.WriteJson(writer);
            }
            if (ScheduleEventId != null) {
                writer.WritePropertyName("scheduleEventId");
                writer.Write(ScheduleEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ScheduleVersion;
            var diff = 0;
            if (CurrentVersion == null && CurrentVersion == other.CurrentVersion)
            {
                // null and null
            }
            else
            {
                diff += CurrentVersion.CompareTo(other.CurrentVersion);
            }
            if (WarningVersion == null && WarningVersion == other.WarningVersion)
            {
                // null and null
            }
            else
            {
                diff += WarningVersion.CompareTo(other.WarningVersion);
            }
            if (ErrorVersion == null && ErrorVersion == other.ErrorVersion)
            {
                // null and null
            }
            else
            {
                diff += ErrorVersion.CompareTo(other.ErrorVersion);
            }
            if (ScheduleEventId == null && ScheduleEventId == other.ScheduleEventId)
            {
                // null and null
            }
            else
            {
                diff += ScheduleEventId.CompareTo(other.ScheduleEventId);
            }
            return diff;
        }

        public void Validate() {
            {
            }
            {
            }
            {
            }
            {
                if (ScheduleEventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("scheduleVersion", "version.scheduleVersion.scheduleEventId.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new ScheduleVersion {
                CurrentVersion = CurrentVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                WarningVersion = WarningVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                ErrorVersion = ErrorVersion.Clone() as Gs2.Gs2Version.Model.Version_,
                ScheduleEventId = ScheduleEventId,
            };
        }
    }
}