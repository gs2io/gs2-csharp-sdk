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
using Gs2.Gs2Dictionary.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Dictionary.Result
{
	[Preserve]
	[System.Serializable]
	public class GetEntryWithSignatureResult : IResult
	{
        public Gs2.Gs2Dictionary.Model.Entry Item { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }

        public GetEntryWithSignatureResult WithItem(Gs2.Gs2Dictionary.Model.Entry item) {
            this.Item = item;
            return this;
        }

        public GetEntryWithSignatureResult WithBody(string body) {
            this.Body = body;
            return this;
        }

        public GetEntryWithSignatureResult WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

    	[Preserve]
        public static GetEntryWithSignatureResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetEntryWithSignatureResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Dictionary.Model.Entry.FromJson(data["item"]))
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["body"] = Body,
                ["signature"] = Signature,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Body != null) {
                writer.WritePropertyName("body");
                writer.Write(Body.ToString());
            }
            if (Signature != null) {
                writer.WritePropertyName("signature");
                writer.Write(Signature.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}