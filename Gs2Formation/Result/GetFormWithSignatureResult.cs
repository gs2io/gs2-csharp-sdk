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
using Gs2.Gs2Formation.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Formation.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetFormWithSignatureResult : IResult
	{
        public Gs2.Gs2Formation.Model.Form Item { set; get; }
        public string Body { set; get; }
        public string Signature { set; get; }
        public Gs2.Gs2Formation.Model.Mold Mold { set; get; }
        public Gs2.Gs2Formation.Model.MoldModel MoldModel { set; get; }
        public Gs2.Gs2Formation.Model.FormModel FormModel { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetFormWithSignatureResult WithItem(Gs2.Gs2Formation.Model.Form item) {
            this.Item = item;
            return this;
        }

        public GetFormWithSignatureResult WithBody(string body) {
            this.Body = body;
            return this;
        }

        public GetFormWithSignatureResult WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

        public GetFormWithSignatureResult WithMold(Gs2.Gs2Formation.Model.Mold mold) {
            this.Mold = mold;
            return this;
        }

        public GetFormWithSignatureResult WithMoldModel(Gs2.Gs2Formation.Model.MoldModel moldModel) {
            this.MoldModel = moldModel;
            return this;
        }

        public GetFormWithSignatureResult WithFormModel(Gs2.Gs2Formation.Model.FormModel formModel) {
            this.FormModel = formModel;
            return this;
        }

        public GetFormWithSignatureResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetFormWithSignatureResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetFormWithSignatureResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Formation.Model.Form.FromJson(data["item"]))
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString())
                .WithMold(!data.Keys.Contains("mold") || data["mold"] == null ? null : Gs2.Gs2Formation.Model.Mold.FromJson(data["mold"]))
                .WithMoldModel(!data.Keys.Contains("moldModel") || data["moldModel"] == null ? null : Gs2.Gs2Formation.Model.MoldModel.FromJson(data["moldModel"]))
                .WithFormModel(!data.Keys.Contains("formModel") || data["formModel"] == null ? null : Gs2.Gs2Formation.Model.FormModel.FromJson(data["formModel"]))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["body"] = Body,
                ["signature"] = Signature,
                ["mold"] = Mold?.ToJson(),
                ["moldModel"] = MoldModel?.ToJson(),
                ["formModel"] = FormModel?.ToJson(),
                ["metadata"] = Metadata?.ToJson(),
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
            if (Mold != null) {
                Mold.WriteJson(writer);
            }
            if (MoldModel != null) {
                MoldModel.WriteJson(writer);
            }
            if (FormModel != null) {
                FormModel.WriteJson(writer);
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}