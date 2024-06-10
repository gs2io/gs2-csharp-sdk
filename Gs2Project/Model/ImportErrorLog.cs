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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class ImportErrorLog : IComparable
	{
        public string DumpProgressId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string MicroserviceName { set; get; } = null!;
        public string Message { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public ImportErrorLog WithDumpProgressId(string dumpProgressId) {
            this.DumpProgressId = dumpProgressId;
            return this;
        }
        public ImportErrorLog WithName(string name) {
            this.Name = name;
            return this;
        }
        public ImportErrorLog WithMicroserviceName(string microserviceName) {
            this.MicroserviceName = microserviceName;
            return this;
        }
        public ImportErrorLog WithMessage(string message) {
            this.Message = message;
            return this;
        }
        public ImportErrorLog WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public ImportErrorLog WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _accountNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):import:(?<transactionId>.+):log:(?<errorLogName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAccountNameFromGrn(
            string grn
        )
        {
            var match = _accountNameRegex.Match(grn);
            if (!match.Success || !match.Groups["accountName"].Success)
            {
                return null;
            }
            return match.Groups["accountName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _projectNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):import:(?<transactionId>.+):log:(?<errorLogName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetProjectNameFromGrn(
            string grn
        )
        {
            var match = _projectNameRegex.Match(grn);
            if (!match.Success || !match.Groups["projectName"].Success)
            {
                return null;
            }
            return match.Groups["projectName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):import:(?<transactionId>.+):log:(?<errorLogName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTransactionIdFromGrn(
            string grn
        )
        {
            var match = _transactionIdRegex.Match(grn);
            if (!match.Success || !match.Groups["transactionId"].Success)
            {
                return null;
            }
            return match.Groups["transactionId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _errorLogNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:::gs2:account:(?<accountName>.+):project:(?<projectName>.+):import:(?<transactionId>.+):log:(?<errorLogName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetErrorLogNameFromGrn(
            string grn
        )
        {
            var match = _errorLogNameRegex.Match(grn);
            if (!match.Success || !match.Groups["errorLogName"].Success)
            {
                return null;
            }
            return match.Groups["errorLogName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ImportErrorLog FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ImportErrorLog()
                .WithDumpProgressId(!data.Keys.Contains("dumpProgressId") || data["dumpProgressId"] == null ? null : data["dumpProgressId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMicroserviceName(!data.Keys.Contains("microserviceName") || data["microserviceName"] == null ? null : data["microserviceName"].ToString())
                .WithMessage(!data.Keys.Contains("message") || data["message"] == null ? null : data["message"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["dumpProgressId"] = DumpProgressId,
                ["name"] = Name,
                ["microserviceName"] = MicroserviceName,
                ["message"] = Message,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (DumpProgressId != null) {
                writer.WritePropertyName("dumpProgressId");
                writer.Write(DumpProgressId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (MicroserviceName != null) {
                writer.WritePropertyName("microserviceName");
                writer.Write(MicroserviceName.ToString());
            }
            if (Message != null) {
                writer.WritePropertyName("message");
                writer.Write(Message.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as ImportErrorLog;
            var diff = 0;
            if (DumpProgressId == null && DumpProgressId == other.DumpProgressId)
            {
                // null and null
            }
            else
            {
                diff += DumpProgressId.CompareTo(other.DumpProgressId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (MicroserviceName == null && MicroserviceName == other.MicroserviceName)
            {
                // null and null
            }
            else
            {
                diff += MicroserviceName.CompareTo(other.MicroserviceName);
            }
            if (Message == null && Message == other.Message)
            {
                // null and null
            }
            else
            {
                diff += Message.CompareTo(other.Message);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
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
                if (DumpProgressId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.dumpProgressId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.name.error.tooLong"),
                    });
                }
            }
            {
                if (MicroserviceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.microserviceName.error.tooLong"),
                    });
                }
            }
            {
                if (Message.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.message.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("importErrorLog", "project.importErrorLog.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new ImportErrorLog {
                DumpProgressId = DumpProgressId,
                Name = Name,
                MicroserviceName = MicroserviceName,
                Message = Message,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}