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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StoreContentModel : IComparable
	{
        public string StoreContentModelId { set; get; } = null!;
        public string Name { set; get; } = null!;
        public string Metadata { set; get; } = null!;
        public Gs2.Gs2Money2.Model.AppleAppStoreContent AppleAppStore { set; get; } = null!;
        public Gs2.Gs2Money2.Model.GooglePlayContent GooglePlay { set; get; } = null!;
        public StoreContentModel WithStoreContentModelId(string storeContentModelId) {
            this.StoreContentModelId = storeContentModelId;
            return this;
        }
        public StoreContentModel WithName(string name) {
            this.Name = name;
            return this;
        }
        public StoreContentModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public StoreContentModel WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreContent appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public StoreContentModel WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlayContent googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:content:(?<contentName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:content:(?<contentName>.+)",
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

        private static System.Text.RegularExpressions.Regex _contentNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):model:content:(?<contentName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetContentNameFromGrn(
            string grn
        )
        {
            var match = _contentNameRegex.Match(grn);
            if (!match.Success || !match.Groups["contentName"].Success)
            {
                return null;
            }
            return match.Groups["contentName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StoreContentModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StoreContentModel()
                .WithStoreContentModelId(!data.Keys.Contains("storeContentModelId") || data["storeContentModelId"] == null ? null : data["storeContentModelId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreContent.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlayContent.FromJson(data["googlePlay"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["storeContentModelId"] = StoreContentModelId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["appleAppStore"] = AppleAppStore?.ToJson(),
                ["googlePlay"] = GooglePlay?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StoreContentModelId != null) {
                writer.WritePropertyName("storeContentModelId");
                writer.Write(StoreContentModelId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (AppleAppStore != null) {
                writer.WritePropertyName("appleAppStore");
                AppleAppStore.WriteJson(writer);
            }
            if (GooglePlay != null) {
                writer.WritePropertyName("googlePlay");
                GooglePlay.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StoreContentModel;
            var diff = 0;
            if (StoreContentModelId == null && StoreContentModelId == other.StoreContentModelId)
            {
                // null and null
            }
            else
            {
                diff += StoreContentModelId.CompareTo(other.StoreContentModelId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (AppleAppStore == null && AppleAppStore == other.AppleAppStore)
            {
                // null and null
            }
            else
            {
                diff += AppleAppStore.CompareTo(other.AppleAppStore);
            }
            if (GooglePlay == null && GooglePlay == other.GooglePlay)
            {
                // null and null
            }
            else
            {
                diff += GooglePlay.CompareTo(other.GooglePlay);
            }
            return diff;
        }

        public void Validate() {
            {
                if (StoreContentModelId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModel", "money2.storeContentModel.storeContentModelId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModel", "money2.storeContentModel.name.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("storeContentModel", "money2.storeContentModel.metadata.error.tooLong"),
                    });
                }
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new StoreContentModel {
                StoreContentModelId = StoreContentModelId,
                Name = Name,
                Metadata = Metadata,
                AppleAppStore = AppleAppStore.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreContent,
                GooglePlay = GooglePlay.Clone() as Gs2.Gs2Money2.Model.GooglePlayContent,
            };
        }
    }
}