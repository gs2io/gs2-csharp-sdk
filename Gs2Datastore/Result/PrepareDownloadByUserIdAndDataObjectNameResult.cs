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
using Gs2.Gs2Datastore.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Datastore.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class PrepareDownloadByUserIdAndDataObjectNameResult : IResult
	{
        public Gs2.Gs2Datastore.Model.DataObject Item { set; get; }
        public string FileUrl { set; get; }
        public long? ContentLength { set; get; }
        public ResultMetadata Metadata { set; get; }

        public PrepareDownloadByUserIdAndDataObjectNameResult WithItem(Gs2.Gs2Datastore.Model.DataObject item) {
            this.Item = item;
            return this;
        }

        public PrepareDownloadByUserIdAndDataObjectNameResult WithFileUrl(string fileUrl) {
            this.FileUrl = fileUrl;
            return this;
        }

        public PrepareDownloadByUserIdAndDataObjectNameResult WithContentLength(long? contentLength) {
            this.ContentLength = contentLength;
            return this;
        }

        public PrepareDownloadByUserIdAndDataObjectNameResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PrepareDownloadByUserIdAndDataObjectNameResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PrepareDownloadByUserIdAndDataObjectNameResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Datastore.Model.DataObject.FromJson(data["item"]))
                .WithFileUrl(!data.Keys.Contains("fileUrl") || data["fileUrl"] == null ? null : data["fileUrl"].ToString())
                .WithContentLength(!data.Keys.Contains("contentLength") || data["contentLength"] == null ? null : (long?)(data["contentLength"].ToString().Contains(".") ? (long)double.Parse(data["contentLength"].ToString()) : long.Parse(data["contentLength"].ToString())))
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["fileUrl"] = FileUrl,
                ["contentLength"] = ContentLength,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (FileUrl != null) {
                writer.WritePropertyName("fileUrl");
                writer.Write(FileUrl.ToString());
            }
            if (ContentLength != null) {
                writer.WritePropertyName("contentLength");
                writer.Write((ContentLength.ToString().Contains(".") ? (long)double.Parse(ContentLength.ToString()) : long.Parse(ContentLength.ToString())));
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}