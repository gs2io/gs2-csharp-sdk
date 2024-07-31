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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TakeOverTypeModel : IComparable
	{
        public string TakeOverTypeModelId { set; get; } = null!;
        public int? Type { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Account.Model.OpenIdConnectSetting OpenIdConnectSetting { set; get; } = null!;
        public TakeOverTypeModel WithTakeOverTypeModelId(string takeOverTypeModelId) {
            this.TakeOverTypeModelId = takeOverTypeModelId;
            return this;
        }
        public TakeOverTypeModel WithType(int? type) {
            this.Type = type;
            return this;
        }
        public TakeOverTypeModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public TakeOverTypeModel WithOpenIdConnectSetting(Gs2.Gs2Account.Model.OpenIdConnectSetting openIdConnectSetting) {
            this.OpenIdConnectSetting = openIdConnectSetting;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):model:takeOver:(?<type>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):model:takeOver:(?<type>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):model:takeOver:(?<type>.+)",
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

        private static System.Text.RegularExpressions.Regex _typeRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):account:(?<namespaceName>.+):model:takeOver:(?<type>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTypeFromGrn(
            string grn
        )
        {
            var match = _typeRegex.Match(grn);
            if (!match.Success || !match.Groups["type"].Success)
            {
                return null;
            }
            return match.Groups["type"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TakeOverTypeModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TakeOverTypeModel()
                .WithTakeOverTypeModelId(!data.Keys.Contains("takeOverTypeModelId") || data["takeOverTypeModelId"] == null ? null : data["takeOverTypeModelId"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : (int?)(data["type"].ToString().Contains(".") ? (int)double.Parse(data["type"].ToString()) : int.Parse(data["type"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithOpenIdConnectSetting(!data.Keys.Contains("openIdConnectSetting") || data["openIdConnectSetting"] == null ? null : Gs2.Gs2Account.Model.OpenIdConnectSetting.FromJson(data["openIdConnectSetting"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["takeOverTypeModelId"] = TakeOverTypeModelId,
                ["type"] = Type,
                ["metadata"] = Metadata,
                ["openIdConnectSetting"] = OpenIdConnectSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TakeOverTypeModelId != null) {
                writer.WritePropertyName("takeOverTypeModelId");
                writer.Write(TakeOverTypeModelId.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write((Type.ToString().Contains(".") ? (int)double.Parse(Type.ToString()) : int.Parse(Type.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (OpenIdConnectSetting != null) {
                writer.WritePropertyName("openIdConnectSetting");
                OpenIdConnectSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TakeOverTypeModel;
            var diff = 0;
            if (TakeOverTypeModelId == null && TakeOverTypeModelId == other.TakeOverTypeModelId)
            {
                // null and null
            }
            else
            {
                diff += TakeOverTypeModelId.CompareTo(other.TakeOverTypeModelId);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += (int)(Type - other.Type);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (OpenIdConnectSetting == null && OpenIdConnectSetting == other.OpenIdConnectSetting)
            {
                // null and null
            }
            else
            {
                diff += OpenIdConnectSetting.CompareTo(other.OpenIdConnectSetting);
            }
            return diff;
        }

        public void Validate() {
            {
                if (TakeOverTypeModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOverTypeModel", "account.takeOverTypeModel.takeOverTypeModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Type < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOverTypeModel", "account.takeOverTypeModel.type.error.invalid"),
                    });
                }
                if (Type > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOverTypeModel", "account.takeOverTypeModel.type.error.invalid"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("takeOverTypeModel", "account.takeOverTypeModel.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
        }

        public object Clone() {
            return new TakeOverTypeModel {
                TakeOverTypeModelId = TakeOverTypeModelId,
                Type = Type,
                Metadata = Metadata,
                OpenIdConnectSetting = OpenIdConnectSetting.Clone() as Gs2.Gs2Account.Model.OpenIdConnectSetting,
            };
        }
    }
}