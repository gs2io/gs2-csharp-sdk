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

namespace Gs2.Gs2Formation.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class SlotWithSignature : IComparable
	{
        public string Name { set; get; }
        public string PropertyType { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }
        public string Metadata { set; get; }
        public SlotWithSignature WithName(string name) {
            this.Name = name;
            return this;
        }
        public SlotWithSignature WithPropertyType(string propertyType) {
            this.PropertyType = propertyType;
            return this;
        }
        public SlotWithSignature WithBody(string body) {
            this.Body = body;
            return this;
        }
        public SlotWithSignature WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }
        public SlotWithSignature WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SlotWithSignature FromJson(JsonData data)
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
            return new SlotWithSignature()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithPropertyType(!data.Keys.Contains("propertyType") || data["propertyType"] == null ? null : data["propertyType"].ToString())
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["propertyType"] = PropertyType,
                ["body"] = Body,
                ["signature"] = Signature,
                ["metadata"] = Metadata,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (PropertyType != null) {
                writer.WritePropertyName("propertyType");
                writer.Write(PropertyType.ToString());
            }
            if (Body != null) {
                writer.WritePropertyName("body");
                writer.Write(Body.ToString());
            }
            if (Signature != null) {
                writer.WritePropertyName("signature");
                writer.Write(Signature.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SlotWithSignature;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type SlotWithSignature.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Name, other.Name);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(PropertyType, other.PropertyType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Body, other.Body);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Signature, other.Signature);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Metadata, other.Metadata);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Name.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotWithSignature", "formation.slotWithSignature.name.error.tooLong"),
                    });
                }
            }
            {
                switch (PropertyType) {
                    case "gs2_inventory":
                    case "gs2_simple_inventory":
                    case "gs2_dictionary":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("slotWithSignature", "formation.slotWithSignature.propertyType.error.invalid"),
                        });
                }
            }
            {
                if (Body.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotWithSignature", "formation.slotWithSignature.body.error.tooLong"),
                    });
                }
            }
            {
                if (Signature.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotWithSignature", "formation.slotWithSignature.signature.error.tooLong"),
                    });
                }
            }
            {
                if (Metadata.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("slotWithSignature", "formation.slotWithSignature.metadata.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SlotWithSignature {
                Name = Name,
                PropertyType = PropertyType,
                Body = Body,
                Signature = Signature,
                Metadata = Metadata,
            };
        }
    }
}