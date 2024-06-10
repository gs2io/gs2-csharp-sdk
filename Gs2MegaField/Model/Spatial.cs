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

namespace Gs2.Gs2MegaField.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Spatial : IComparable
	{
        public string SpatialId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string AreaModelName { set; get; } = null!;
        public string LayerModelName { set; get; } = null!;
        public Gs2.Gs2MegaField.Model.Position Position { set; get; } = null!;
        public Gs2.Gs2MegaField.Model.Vector Vector { set; get; } = null!;
        public float? R { set; get; } = null!;
        public long? LastSyncAt { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public Spatial WithSpatialId(string spatialId) {
            this.SpatialId = spatialId;
            return this;
        }
        public Spatial WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Spatial WithAreaModelName(string areaModelName) {
            this.AreaModelName = areaModelName;
            return this;
        }
        public Spatial WithLayerModelName(string layerModelName) {
            this.LayerModelName = layerModelName;
            return this;
        }
        public Spatial WithPosition(Gs2.Gs2MegaField.Model.Position position) {
            this.Position = position;
            return this;
        }
        public Spatial WithVector(Gs2.Gs2MegaField.Model.Vector vector) {
            this.Vector = vector;
            return this;
        }
        public Spatial WithR(float? r) {
            this.R = r;
            return this;
        }
        public Spatial WithLastSyncAt(long? lastSyncAt) {
            this.LastSyncAt = lastSyncAt;
            return this;
        }
        public Spatial WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
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

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _areaModelNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):megaField:(?<namespaceName>.+):user:(?<userId>.+):spatial:(?<areaModelName>.+):(?<layerModelName>.+)",
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
        public static Spatial FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Spatial()
                .WithSpatialId(!data.Keys.Contains("spatialId") || data["spatialId"] == null ? null : data["spatialId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAreaModelName(!data.Keys.Contains("areaModelName") || data["areaModelName"] == null ? null : data["areaModelName"].ToString())
                .WithLayerModelName(!data.Keys.Contains("layerModelName") || data["layerModelName"] == null ? null : data["layerModelName"].ToString())
                .WithPosition(!data.Keys.Contains("position") || data["position"] == null ? null : Gs2.Gs2MegaField.Model.Position.FromJson(data["position"]))
                .WithVector(!data.Keys.Contains("vector") || data["vector"] == null ? null : Gs2.Gs2MegaField.Model.Vector.FromJson(data["vector"]))
                .WithR(!data.Keys.Contains("r") || data["r"] == null ? null : (float?)float.Parse(data["r"].ToString()))
                .WithLastSyncAt(!data.Keys.Contains("lastSyncAt") || data["lastSyncAt"] == null ? null : (long?)(data["lastSyncAt"].ToString().Contains(".") ? (long)double.Parse(data["lastSyncAt"].ToString()) : long.Parse(data["lastSyncAt"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["spatialId"] = SpatialId,
                ["userId"] = UserId,
                ["areaModelName"] = AreaModelName,
                ["layerModelName"] = LayerModelName,
                ["position"] = Position?.ToJson(),
                ["vector"] = Vector?.ToJson(),
                ["r"] = R,
                ["lastSyncAt"] = LastSyncAt,
                ["createdAt"] = CreatedAt,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SpatialId != null) {
                writer.WritePropertyName("spatialId");
                writer.Write(SpatialId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (AreaModelName != null) {
                writer.WritePropertyName("areaModelName");
                writer.Write(AreaModelName.ToString());
            }
            if (LayerModelName != null) {
                writer.WritePropertyName("layerModelName");
                writer.Write(LayerModelName.ToString());
            }
            if (Position != null) {
                writer.WritePropertyName("position");
                Position.WriteJson(writer);
            }
            if (Vector != null) {
                writer.WritePropertyName("vector");
                Vector.WriteJson(writer);
            }
            if (R != null) {
                writer.WritePropertyName("r");
                writer.Write(float.Parse(R.ToString()));
            }
            if (LastSyncAt != null) {
                writer.WritePropertyName("lastSyncAt");
                writer.Write((LastSyncAt.ToString().Contains(".") ? (long)double.Parse(LastSyncAt.ToString()) : long.Parse(LastSyncAt.ToString())));
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Spatial;
            var diff = 0;
            if (SpatialId == null && SpatialId == other.SpatialId)
            {
                // null and null
            }
            else
            {
                diff += SpatialId.CompareTo(other.SpatialId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (AreaModelName == null && AreaModelName == other.AreaModelName)
            {
                // null and null
            }
            else
            {
                diff += AreaModelName.CompareTo(other.AreaModelName);
            }
            if (LayerModelName == null && LayerModelName == other.LayerModelName)
            {
                // null and null
            }
            else
            {
                diff += LayerModelName.CompareTo(other.LayerModelName);
            }
            if (Position == null && Position == other.Position)
            {
                // null and null
            }
            else
            {
                diff += Position.CompareTo(other.Position);
            }
            if (Vector == null && Vector == other.Vector)
            {
                // null and null
            }
            else
            {
                diff += Vector.CompareTo(other.Vector);
            }
            if (R == null && R == other.R)
            {
                // null and null
            }
            else
            {
                diff += (int)(R - other.R);
            }
            if (LastSyncAt == null && LastSyncAt == other.LastSyncAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(LastSyncAt - other.LastSyncAt);
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
                if (SpatialId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.spatialId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.userId.error.tooLong"),
                    });
                }
            }
            {
                if (AreaModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.areaModelName.error.tooLong"),
                    });
                }
            }
            {
                if (LayerModelName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.layerModelName.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
            {
                if (R < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.r.error.invalid"),
                    });
                }
                if (R > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.r.error.invalid"),
                    });
                }
            }
            {
                if (LastSyncAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.lastSyncAt.error.invalid"),
                    });
                }
                if (LastSyncAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.lastSyncAt.error.invalid"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("spatial", "megaField.spatial.createdAt.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Spatial {
                SpatialId = SpatialId,
                UserId = UserId,
                AreaModelName = AreaModelName,
                LayerModelName = LayerModelName,
                Position = Position.Clone() as Gs2.Gs2MegaField.Model.Position,
                Vector = Vector.Clone() as Gs2.Gs2MegaField.Model.Vector,
                R = R,
                LastSyncAt = LastSyncAt,
                CreatedAt = CreatedAt,
            };
        }
    }
}