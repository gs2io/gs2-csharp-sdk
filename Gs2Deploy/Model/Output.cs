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

namespace Gs2.Gs2Deploy.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Output : IComparable
	{
        public string OutputId { set; get; }
        public string Name { set; get; }
        public string Value { set; get; }
        public long? CreatedAt { set; get; }
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
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Output()
                .WithOutputId(!data.Keys.Contains("outputId") || data["outputId"] == null ? null : data["outputId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithValue(!data.Keys.Contains("value") || data["value"] == null ? null : data["value"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()));
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Output.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(OutputId, other.OutputId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Value, other.Value);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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