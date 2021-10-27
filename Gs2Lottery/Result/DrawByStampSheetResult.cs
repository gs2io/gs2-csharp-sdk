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
using Gs2.Gs2Lottery.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Lottery.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DrawByStampSheetResult : IResult
	{
        public Gs2.Gs2Lottery.Model.DrawnPrize[] Items { set; get; }
        public string StampSheet { set; get; }
        public string StampSheetEncryptionKeyId { set; get; }
        public Gs2.Gs2Lottery.Model.BoxItems BoxItems { set; get; }

        public DrawByStampSheetResult WithItems(Gs2.Gs2Lottery.Model.DrawnPrize[] items) {
            this.Items = items;
            return this;
        }

        public DrawByStampSheetResult WithStampSheet(string stampSheet) {
            this.StampSheet = stampSheet;
            return this;
        }

        public DrawByStampSheetResult WithStampSheetEncryptionKeyId(string stampSheetEncryptionKeyId) {
            this.StampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
            return this;
        }

        public DrawByStampSheetResult WithBoxItems(Gs2.Gs2Lottery.Model.BoxItems boxItems) {
            this.BoxItems = boxItems;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DrawByStampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DrawByStampSheetResult()
                .WithItems(!data.Keys.Contains("items") || data["items"] == null ? new Gs2.Gs2Lottery.Model.DrawnPrize[]{} : data["items"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Lottery.Model.DrawnPrize.FromJson(v);
                }).ToArray())
                .WithStampSheet(!data.Keys.Contains("stampSheet") || data["stampSheet"] == null ? null : data["stampSheet"].ToString())
                .WithStampSheetEncryptionKeyId(!data.Keys.Contains("stampSheetEncryptionKeyId") || data["stampSheetEncryptionKeyId"] == null ? null : data["stampSheetEncryptionKeyId"].ToString())
                .WithBoxItems(!data.Keys.Contains("boxItems") || data["boxItems"] == null ? null : Gs2.Gs2Lottery.Model.BoxItems.FromJson(data["boxItems"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["items"] = new JsonData(Items == null ? new JsonData[]{} :
                        Items.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["stampSheet"] = StampSheet,
                ["stampSheetEncryptionKeyId"] = StampSheetEncryptionKeyId,
                ["boxItems"] = BoxItems?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            writer.WriteArrayStart();
            foreach (var item in Items)
            {
                if (item != null) {
                    item.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            if (StampSheet != null) {
                writer.WritePropertyName("stampSheet");
                writer.Write(StampSheet.ToString());
            }
            if (StampSheetEncryptionKeyId != null) {
                writer.WritePropertyName("stampSheetEncryptionKeyId");
                writer.Write(StampSheetEncryptionKeyId.ToString());
            }
            if (BoxItems != null) {
                BoxItems.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}