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

namespace Gs2.Gs2Distributor.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class UserDataEntry : IComparable
	{
        public string Service { set; get; }
        public string NamespaceName { set; get; }
        public string Kind { set; get; }
        public string Payload { set; get; }
        public UserDataEntry WithService(string service) {
            this.Service = service;
            return this;
        }
        public UserDataEntry WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UserDataEntry WithKind(string kind) {
            this.Kind = kind;
            return this;
        }
        public UserDataEntry WithPayload(string payload) {
            this.Payload = payload;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UserDataEntry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new UserDataEntry()
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithKind(!data.Keys.Contains("kind") || data["kind"] == null ? null : data["kind"].ToString())
                .WithPayload(!data.Keys.Contains("payload") || data["payload"] == null ? null : data["payload"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["service"] = Service,
                ["namespaceName"] = NamespaceName,
                ["kind"] = Kind,
                ["payload"] = Payload,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Kind != null) {
                writer.WritePropertyName("kind");
                writer.Write(Kind.ToString());
            }
            if (Payload != null) {
                writer.WritePropertyName("payload");
                writer.Write(Payload.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as UserDataEntry;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type UserDataEntry.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(Service, other.Service);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(NamespaceName, other.NamespaceName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Kind, other.Kind);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Payload, other.Payload);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (Service.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("userDataEntry", "distributor.userDataEntry.service.error.tooLong"),
                    });
                }
            }
            {
                if (NamespaceName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("userDataEntry", "distributor.userDataEntry.namespaceName.error.tooLong"),
                    });
                }
            }
            {
                if (Kind.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("userDataEntry", "distributor.userDataEntry.kind.error.tooLong"),
                    });
                }
            }
            {
                if (Payload.Length > 409600) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("userDataEntry", "distributor.userDataEntry.payload.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new UserDataEntry {
                Service = Service,
                NamespaceName = NamespaceName,
                Kind = Kind,
                Payload = Payload,
            };
        }
    }
}