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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Output : IComparable
	{
        public string OutputId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Value { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public Output WithOutputId(string outputId) {
            this.OutputId = outputId;
            return this;
        }
        public Output WithName(string name) {
            this.Name = name;
            return this;
        }
        public Output WithValue(string value) {
            this.Value = value;
            return this;
        }
        public Output WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):output:(?<outputName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):output:(?<outputName>.+)",
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

        private static System.Text.RegularExpressions.Regex _stackNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):output:(?<outputName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetStackNameFromGrn(
            string grn
        )
        {
            var match = _stackNameRegex.Match(grn);
            if (!match.Success || !match.Groups["stackName"].Success)
            {
                return null;
            }
            return match.Groups["stackName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _outputNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):deploy:(?<stackName>.+):output:(?<outputName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOutputNameFromGrn(
            string grn
        )
        {
            var match = _outputNameRegex.Match(grn);
            if (!match.Success || !match.Groups["outputName"].Success)
            {
                return null;
            }
            return match.Groups["outputName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Output FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Output()
                .WithOutputId(!data.Keys.Contains("outputId") || data["outputId"] == null ? null : data["outputId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : data["value"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["outputId"] = OutputId,
                ["name"] = Name,
                ["value"] = Value,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (OutputId != null) {
                writer.WritePropertyName("outputId");
                writer.Write(OutputId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Value != null) {
                writer.WritePropertyName("value");
                writer.Write(Value.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Output;
            var diff = 0;
            if (OutputId == null && OutputId == other.OutputId)
            {
                // null and null
            }
            else
            {
                diff += OutputId.CompareTo(other.OutputId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Value == null && Value == other.Value)
            {
                // null and null
            }
            else
            {
                diff += Value.CompareTo(other.Value);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            return diff;
        }

        public void Validate() {
            {
                if (OutputId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("output", "deploy.output.outputId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("output", "deploy.output.name.error.tooLong"),
                    });
                }
            }
            {
                if (Value.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("output", "deploy.output.value.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("output", "deploy.output.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("output", "deploy.output.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Output {
                OutputId = OutputId,
                Name = Name,
                Value = Value,
                CreatedAt = CreatedAt,
            };
        }
    }
}