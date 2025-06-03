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
using Gs2.Gs2Freeze.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Freeze.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetStageResult : IResult
	{
        public Gs2.Gs2Freeze.Model.Stage Item { set; get; }
        public Gs2.Gs2Freeze.Model.Microservice[] Source { set; get; }
        public Gs2.Gs2Freeze.Model.Microservice[] Current { set; get; }
        public ResultMetadata Metadata { set; get; }

        public GetStageResult WithItem(Gs2.Gs2Freeze.Model.Stage item) {
            this.Item = item;
            return this;
        }

        public GetStageResult WithSource(Gs2.Gs2Freeze.Model.Microservice[] source) {
            this.Source = source;
            return this;
        }

        public GetStageResult WithCurrent(Gs2.Gs2Freeze.Model.Microservice[] current) {
            this.Current = current;
            return this;
        }

        public GetStageResult WithMetadata(ResultMetadata metadata) {
            this.Metadata = metadata;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetStageResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetStageResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2Freeze.Model.Stage.FromJson(data["item"]))
                .WithSource(!data.Keys.Contains("source") || data["source"] == null || !data["source"].IsArray ? null : data["source"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Freeze.Model.Microservice.FromJson(v);
                }).ToArray())
                .WithCurrent(!data.Keys.Contains("current") || data["current"] == null || !data["current"].IsArray ? null : data["current"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Freeze.Model.Microservice.FromJson(v);
                }).ToArray())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : ResultMetadata.FromJson(data["metadata"]));
        }

        public JsonData ToJson()
        {
            JsonData sourceJsonData = null;
            if (Source != null && Source.Length > 0)
            {
                sourceJsonData = new JsonData();
                foreach (var sourc in Source)
                {
                    sourceJsonData.Add(sourc.ToJson());
                }
            }
            JsonData currentJsonData = null;
            if (Current != null && Current.Length > 0)
            {
                currentJsonData = new JsonData();
                foreach (var curren in Current)
                {
                    currentJsonData.Add(curren.ToJson());
                }
            }
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["source"] = sourceJsonData,
                ["current"] = currentJsonData,
                ["metadata"] = Metadata?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Source != null) {
                writer.WritePropertyName("source");
                writer.WriteArrayStart();
                foreach (var sourc in Source)
                {
                    if (sourc != null) {
                        sourc.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Current != null) {
                writer.WritePropertyName("current");
                writer.WriteArrayStart();
                foreach (var curren in Current)
                {
                    if (curren != null) {
                        curren.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                Metadata.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }
    }
}