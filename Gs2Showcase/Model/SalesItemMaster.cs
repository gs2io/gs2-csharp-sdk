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

namespace Gs2.Gs2Showcase.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SalesItemMaster : IComparable
	{
        public string SalesItemId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public Gs2.Core.Model.VerifyAction[] VerifyActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public SalesItemMaster WithSalesItemId(string salesItemId) {
            this.SalesItemId = salesItemId;
            return this;
        }
        public SalesItemMaster WithName(string name) {
            this.Name = name;
            return this;
        }
        public SalesItemMaster WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public SalesItemMaster WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public SalesItemMaster WithVerifyActions(Gs2.Core.Model.VerifyAction[] verifyActions) {
            this.VerifyActions = verifyActions;
            return this;
        }
        public SalesItemMaster WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }
        public SalesItemMaster WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public SalesItemMaster WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public SalesItemMaster WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public SalesItemMaster WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItem:(?<salesItemName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItem:(?<salesItemName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItem:(?<salesItemName>.+)",
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

        private static System.Text.RegularExpressions.Regex _salesItemNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):showcase:(?<namespaceName>.+):salesItem:(?<salesItemName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetSalesItemNameFromGrn(
            string grn
        )
        {
            var match = _salesItemNameRegex.Match(grn);
            if (!match.Success || !match.Groups["salesItemName"].Success)
            {
                return null;
            }
            return match.Groups["salesItemName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SalesItemMaster FromJson(JsonData data)
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
            return new SalesItemMaster()
                .WithSalesItemId(!data.Keys.Contains("salesItemId") || data["salesItemId"] == null ? null : data["salesItemId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithVerifyActions(!data.Keys.Contains("verifyActions") || data["verifyActions"] == null || !data["verifyActions"].IsArray ? null : data["verifyActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null || !data["consumeActions"].IsArray ? null : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? null : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["updatedAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
        }

        public JsonData ToJson()
        {
            JsonData verifyActionsJsonData = null;
            if (VerifyActions != null && VerifyActions.Length > 0)
            {
                verifyActionsJsonData = new JsonData();
                foreach (var verifyAction in VerifyActions)
                {
                    verifyActionsJsonData.Add(verifyAction.ToJson());
                }
            }
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null && ConsumeActions.Length > 0)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null && AcquireActions.Length > 0)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["salesItemId"] = SalesItemId,
                ["name"] = Name,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["verifyActions"] = verifyActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (SalesItemId != null) {
                writer.WritePropertyName("salesItemId");
                writer.Write(SalesItemId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (VerifyActions != null) {
                writer.WritePropertyName("verifyActions");
                writer.WriteArrayStart();
                foreach (var verifyAction in VerifyActions)
                {
                    if (verifyAction != null) {
                        verifyAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ConsumeActions != null) {
                writer.WritePropertyName("consumeActions");
                writer.WriteArrayStart();
                foreach (var consumeAction in ConsumeActions)
                {
                    if (consumeAction != null) {
                        consumeAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquireActions != null) {
                writer.WritePropertyName("acquireActions");
                writer.WriteArrayStart();
                foreach (var acquireAction in AcquireActions)
                {
                    if (acquireAction != null) {
                        acquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            var other = obj as SalesItemMaster;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type SalesItemMaster.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(SalesItemId, other.SalesItemId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Description, other.Description);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(VerifyActions, other.VerifyActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(ConsumeActions, other.ConsumeActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.CompareArray(AcquireActions, other.AcquireActions);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UpdatedAt, other.UpdatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (SalesItemId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.salesItemId.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.name.error.tooLong"),
                    });
                }
            }
            {
                if (Description.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.description.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 2048) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.verifyActions.error.tooMany"),
                    });
                }
            }
            {
                if (ConsumeActions.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.consumeActions.error.tooMany"),
                    });
                }
            }
            {
                if (AcquireActions.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.acquireActions.error.tooFew"),
                    });
                }
                if (AcquireActions.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.acquireActions.error.tooMany"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (UpdatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.updatedAt.error.invalid"),
                    });
                }
                if (UpdatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.updatedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("salesItemMaster", "showcase.salesItemMaster.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new SalesItemMaster {
                SalesItemId = SalesItemId,
                Name = Name,
                Description = Description,
                Metadata = Metadata,
                VerifyActions = VerifyActions?.Clone() as Gs2.Core.Model.VerifyAction[],
                ConsumeActions = ConsumeActions?.Clone() as Gs2.Core.Model.ConsumeAction[],
                AcquireActions = AcquireActions?.Clone() as Gs2.Core.Model.AcquireAction[],
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Revision = Revision,
            };
        }
    }
}