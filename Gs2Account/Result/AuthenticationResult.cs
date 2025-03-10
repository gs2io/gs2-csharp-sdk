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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Account.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Account.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class AuthenticationResult : IResult
	{
        public Gs2.Gs2Account.Model.Account Item { set; get; }
        public Gs2.Gs2Account.Model.BanStatus[] BanStatuses { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }
        public ResultMetadata Metadata { set; get; }

        public AuthenticationResult WithItem(Gs2.Gs2Account.Model.Account item) {
            this.Item = item;
            return this;
        }

        public AuthenticationResult WithBanStatuses(Gs2.Gs2Account.Model.BanStatus[] banStatuses) {
            this.BanStatuses = banStatuses;
            return this;
        }

        public AuthenticationResult WithBody(string body) {
            this.Body = body;
            return this;
        }

        public AuthenticationResult WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

        public AuthenticationResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AuthenticationResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AuthenticationResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Account.Model.Account.FromJson(data["item"]))
                .WithBanStatuses(!data.Keys.Contains("banStatuses") || data["banStatuses"] == null || !data["banStatuses"].IsArray ? null : data["banStatuses"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Account.Model.BanStatus.FromJson(v);
                }).ToArray())
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            JsonData banStatusesJsonData = null;
            if (BanStatuses != null && BanStatuses.Length > 0)
            {
                banStatusesJsonData = new JsonData();
                foreach (var banStatus in BanStatuses)
                {
                    banStatusesJsonData.Add(banStatus.ToJson());
                }
            }
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["banStatuses"] = banStatusesJsonData,
                ["body"] = Body,
                ["signature"] = Signature,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (BanStatuses != null) {
                writer.WritePropertyName("banStatuses");
                writer.WriteArrayStart();
                foreach (var banStatus in BanStatuses)
                {
                    if (banStatus != null) {
                        banStatus.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}