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

namespace Gs2.Gs2Identifier.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Identifier : IComparable
	{
        public string ClientId { set; get; } = null!;
        public string UserName { set; get; } = null!;
        public string ClientSecret { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Identifier WithClientId(string clientId) {
            this.ClientId = clientId;
            return this;
        }
        public Identifier WithUserName(string userName) {
            this.UserName = userName;
            return this;
        }
        public Identifier WithClientSecret(string clientSecret) {
            this.ClientSecret = clientSecret;
            return this;
        }
        public Identifier WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Identifier WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Identifier FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Identifier()
                .WithClientId(!data.Keys.Contains("clientId") || data["clientId"] == null ? null : data["clientId"].ToString())
                .WithUserName(!data.Keys.Contains("userName") || data["userName"] == null ? null : data["userName"].ToString())
                .WithClientSecret(!data.Keys.Contains("clientSecret") || data["clientSecret"] == null ? null : data["clientSecret"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["clientId"] = ClientId,
                ["userName"] = UserName,
                ["clientSecret"] = ClientSecret,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ClientId != null) {
                writer.WritePropertyName("clientId");
                writer.Write(ClientId.ToString());
            }
            if (UserName != null) {
                writer.WritePropertyName("userName");
                writer.Write(UserName.ToString());
            }
            if (ClientSecret != null) {
                writer.WritePropertyName("clientSecret");
                writer.Write(ClientSecret.ToString());
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
            var other = obj as Identifier;
            var diff = 0;
            if (ClientId == null && ClientId == other.ClientId)
            {
                // null and null
            }
            else
            {
                diff += ClientId.CompareTo(other.ClientId);
            }
            if (UserName == null && UserName == other.UserName)
            {
                // null and null
            }
            else
            {
                diff += UserName.CompareTo(other.UserName);
            }
            if (ClientSecret == null && ClientSecret == other.ClientSecret)
            {
                // null and null
            }
            else
            {
                diff += ClientSecret.CompareTo(other.ClientSecret);
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
                if (ClientId.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.clientId.error.tooLong"),
                    });
                }
            }
            {
                if (UserName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.userName.error.tooLong"),
                    });
                }
            }
            {
                if (ClientSecret.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.clientSecret.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("identifier", "identifier.identifier.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Identifier {
                ClientId = ClientId,
                UserName = UserName,
                ClientSecret = ClientSecret,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}