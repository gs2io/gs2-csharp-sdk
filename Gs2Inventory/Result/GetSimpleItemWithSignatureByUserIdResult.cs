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
using Gs2.Gs2Inventory.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inventory.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetSimpleItemWithSignatureByUserIdResult : IResult
	{
        public Gs2.Gs2Inventory.Model.SimpleItem Item { set; get; }
        public Gs2.Gs2Inventory.Model.SimpleItemModel SimpleItemModel { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetSimpleItemWithSignatureByUserIdResult WithItem(Gs2.Gs2Inventory.Model.SimpleItem item) {
            this.Item = item;
            return this;
        }

        public GetSimpleItemWithSignatureByUserIdResult WithSimpleItemModel(Gs2.Gs2Inventory.Model.SimpleItemModel simpleItemModel) {
            this.SimpleItemModel = simpleItemModel;
            return this;
        }

        public GetSimpleItemWithSignatureByUserIdResult WithBody(string body) {
            this.Body = body;
            return this;
        }

        public GetSimpleItemWithSignatureByUserIdResult WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

        public GetSimpleItemWithSignatureByUserIdResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetSimpleItemWithSignatureByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetSimpleItemWithSignatureByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Inventory.Model.SimpleItem.FromJson(data["item"]))
                .WithSimpleItemModel(!data.Keys.Contains("simpleItemModel") || data["simpleItemModel"] == null ? null : Gs2.Gs2Inventory.Model.SimpleItemModel.FromJson(data["simpleItemModel"]))
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["simpleItemModel"] = SimpleItemModel?.ToJson(),
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
            if (SimpleItemModel != null) {
                SimpleItemModel.WriteJson(writer);
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