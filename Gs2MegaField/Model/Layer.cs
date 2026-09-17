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

namespace Gs2.Gs2MegaField.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Layer : IComparable
	{
        public string LayerId { set; get; }
        public string AreaModelName { set; get; }
        public string LayerModelName { set; get; }
        public int? NumberOfMinEntries { set; get; }
        public int? NumberOfMaxEntries { set; get; }
        public long? CreatedAt { set; get; }
        public Layer WithLayerId(string layerId) {
            this.LayerId = layerId;
            return this;
        }
        public Layer WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public Layer WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public Layer WithNumberOfMinEntries(int? numberOfMinEntries) {
            this.NumberOfMinEntries = numberOfMinEntries;
            return this;
        }
        public Layer WithNumberOfMaxEntries(int? numberOfMaxEntries) {
            this.NumberOfMaxEntries = numberOfMaxEntries;
            return this;
        }
        public Layer WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):layer:(?<areaModelName>.+):(?<layerModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):layer:(?<areaModelName>.+):(?<layerModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):layer:(?<areaModelName>.+):(?<layerModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _areaModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):layer:(?<areaModelName>.+):(?<layerModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAreaModelNameFromGrn(
            string grn
        )
        {
            var match = _areaModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["areaModelName"].Success)
            {
                return null;
            }
            return match.Groups["areaModelName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _layerModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):layer:(?<areaModelName>.+):(?<layerModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetLayerModelNameFromGrn(
            string grn
        )
        {
            var match = _layerModelNameRegex.Match(grn);
            if (!match.Success || !match.Groups["layerModelName"].Success)
            {
                return null;
            }
            return match.Groups["layerModelName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Layer FromJson(JsonData data)
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
            return new Layer()
                .WithLayerId(!data.Keys.Contains("layerId") || data["layerId"] == null ? null : data["layerId"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithNumberOfMinEntries(!data.Keys.Contains("numberOfMinEntries") || data["numberOfMinEntries"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["numberOfMinEntries"].ToString()))
                .WithNumberOfMaxEntries(!data.Keys.Contains("numberOfMaxEntries") || data["numberOfMaxEntries"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableInt(data["numberOfMaxEntries"].ToString()))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["layerId"] = LayerId,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["numberOfMinEntries"] = NumberOfMinEntries,
                ["numberOfMaxEntries"] = NumberOfMaxEntries,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (LayerId != null) {
                writer.WritePropertyName("layerId");
                writer.Write(LayerId.ToString());
            }
            if (AreaModelName != null) {
                writer.WritePropertyName("areaModelName");
                writer.Write(AreaModelName.ToString());
            }
            if (LayerModelName != null) {
                writer.WritePropertyName("layerModelName");
                writer.Write(LayerModelName.ToString());
            }
            if (NumberOfMinEntries != null) {
                writer.WritePropertyName("numberOfMinEntries");
                writer.Write((NumberOfMinEntries.ToString().Contains(".") ? (int)double.Parse(NumberOfMinEntries.ToString()) : int.Parse(NumberOfMinEntries.ToString())));
            }
            if (NumberOfMaxEntries != null) {
                writer.WritePropertyName("numberOfMaxEntries");
                writer.Write((NumberOfMaxEntries.ToString().Contains(".") ? (int)double.Parse(NumberOfMaxEntries.ToString()) : int.Parse(NumberOfMaxEntries.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Layer;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Layer.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(LayerId, other.LayerId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(AreaModelName, other.AreaModelName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(LayerModelName, other.LayerModelName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NumberOfMinEntries, other.NumberOfMinEntries);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NumberOfMaxEntries, other.NumberOfMaxEntries);
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
                if (LayerId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.layerId.error.tooLong"),
                    });
                }
            }
            {
                if (AreaModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.areaModelName.error.tooLong"),
                    });
                }
            }
            {
                if (LayerModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.layerModelName.error.tooLong"),
                    });
                }
            }
            {
                if (NumberOfMinEntries < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.numberOfMinEntries.error.invalid"),
                    });
                }
                if (NumberOfMinEntries > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.numberOfMinEntries.error.invalid"),
                    });
                }
            }
            {
                if (NumberOfMaxEntries < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.numberOfMaxEntries.error.invalid"),
                    });
                }
                if (NumberOfMaxEntries > 1000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.numberOfMaxEntries.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("layer", "megaField.layer.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Layer {
                LayerId = LayerId,
                AreaModelName = AreaModelName,
                LayerModelName = LayerModelName,
                NumberOfMinEntries = NumberOfMinEntries,
                NumberOfMaxEntries = NumberOfMaxEntries,
                CreatedAt = CreatedAt,
            };
        }
    }
}