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
using Gs2.Gs2Inbox.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Inbox.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ReadMessageByUserIdResult : IResult
	{
        public Gs2.Gs2Inbox.Model.Message Item { set; get; }
        public string StampSheet { set; get; }
        public string StampSheetEncryptionKeyId { set; get; }

        public ReadMessageByUserIdResult WithItem(Gs2.Gs2Inbox.Model.Message item) {
            this.Item = item;
            return this;
        }

        public ReadMessageByUserIdResult WithStampSheet(string stampSheet) {
            this.StampSheet = stampSheet;
            return this;
        }

        public ReadMessageByUserIdResult WithStampSheetEncryptionKeyId(string stampSheetEncryptionKeyId) {
            this.StampSheetEncryptionKeyId = stampSheetEncryptionKeyId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReadMessageByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReadMessageByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Inbox.Model.Message.FromJson(data["item"]))
                .WithStampSheet(!data.Keys.Contains("stampSheet") || data["stampSheet"] == null ? null : data["stampSheet"].ToString())
                .WithStampSheetEncryptionKeyId(!data.Keys.Contains("stampSheetEncryptionKeyId") || data["stampSheetEncryptionKeyId"] == null ? null : data["stampSheetEncryptionKeyId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["stampSheet"] = StampSheet,
                ["stampSheetEncryptionKeyId"] = StampSheetEncryptionKeyId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (StampSheet != null) {
                writer.WritePropertyName("stampSheet");
                writer.Write(StampSheet.ToString());
            }
            if (StampSheetEncryptionKeyId != null) {
                writer.WritePropertyName("stampSheetEncryptionKeyId");
                writer.Write(StampSheetEncryptionKeyId.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}