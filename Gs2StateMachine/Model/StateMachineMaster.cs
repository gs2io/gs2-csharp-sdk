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

namespace Gs2.Gs2StateMachine.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StateMachineMaster : IComparable
	{
        public string StateMachineId { set; get; } = null!;
        public string MainStateMachineName { set; get; } = null!;
        public string Payload { set; get; } = null!;
        public long? Version { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? UpdatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public StateMachineMaster WithStateMachineId(string stateMachineId) {
            this.StateMachineId = stateMachineId;
            return this;
        }
        public StateMachineMaster WithMainStateMachineName(string mainStateMachineName) {
            this.MainStateMachineName = mainStateMachineName;
            return this;
        }
        public StateMachineMaster WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }
        public StateMachineMaster WithVersion(long? version) {
            this.Version = version;
            return this;
        }
        public StateMachineMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public StateMachineMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public StateMachineMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):master:stateMachine:(?<version>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):master:stateMachine:(?<version>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):master:stateMachine:(?<version>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _versionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):stateMachine:(?<namespaceName>.+):master:stateMachine:(?<version>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetVersionFromGrn(
            string grn
        )
        {
            var match = _versionRegex.Match(grn);
            if (!match.Success || !match.Groups["version"].Success)
            {
                return null;
            }
            return match.Groups["version"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StateMachineMaster FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StateMachineMaster()
                .WithStateMachineId(!data.Keys.Contains("stateMachineId") || data["stateMachineId"] == null ? null : data["stateMachineId"].ToString())
                .WithMainStateMachineName(!data.Keys.Contains("mainStateMachineName") || data["mainStateMachineName"] == null ? null : data["mainStateMachineName"].ToString())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString())
                .WithVersion(!data.Keys.Contains("version") || data["version"] == null ? null : (long?)(data["version"].ToString().Contains(".") ? (long)double.Parse(data["version"].ToString()) : long.Parse(data["version"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["stateMachineId"] = StateMachineId,
                ["mainStateMachineName"] = MainStateMachineName,
                ["payload"] = Payload,
                ["version"] = Version,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StateMachineId != null) {
                writer.WritePropertyName("stateMachineId");
                writer.Write(StateMachineId.ToString());
            }
            if (MainStateMachineName != null) {
                writer.WritePropertyName("mainStateMachineName");
                writer.Write(MainStateMachineName.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            if (Version != null) {
                writer.WritePropertyName("version");
                writer.Write((Version.ToString().Contains(".") ? (long)double.Parse(Version.ToString()) : long.Parse(Version.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StateMachineMaster;
            var diff = 0;
            if (StateMachineId == null && StateMachineId == other.StateMachineId)
            {
                // null and null
            }
            else
            {
                diff += StateMachineId.CompareTo(other.StateMachineId);
            }
            if (MainStateMachineName == null && MainStateMachineName == other.MainStateMachineName)
            {
                // null and null
            }
            else
            {
                diff += MainStateMachineName.CompareTo(other.MainStateMachineName);
            }
            if (Payload == null && Payload == other.Payload)
            {
                // null and null
            }
            else
            {
                diff += Payload.CompareTo(other.Payload);
            }
            if (Version == null && Version == other.Version)
            {
                // null and null
            }
            else
            {
                diff += (int)(Version - other.Version);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }

        public void Validate() {
            {
                if (StateMachineId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.stateMachineId.error.tooLong"),
                    });
                }
            }
            {
                if (MainStateMachineName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.mainStateMachineName.error.tooLong"),
                    });
                }
            }
            {
                if (Payload.Length > 5242880) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.payload.error.tooLong"),
                    });
                }
            }
            {
                if (Version < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.version.error.invalid"),
                    });
                }
                if (Version > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.version.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stateMachineMaster", "stateMachine.stateMachineMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new StateMachineMaster {
                StateMachineId = StateMachineId,
                MainStateMachineName = MainStateMachineName,
                Payload = Payload,
                Version = Version,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}