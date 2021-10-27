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
using Gs2.Gs2Watch.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Watch.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class GetChartRequest : Gs2Request<GetChartRequest>
	{
        public string Metrics { set; get; }
        public string Grn { set; get; }
        public string[] Queries { set; get; }
        public string By { set; get; }
        public string Timeframe { set; get; }
        public string Size { set; get; }
        public string Format { set; get; }
        public string Aggregator { set; get; }
        public string Style { set; get; }
        public string Title { set; get; }

        public GetChartRequest WithMetrics(string metrics) {
            this.Metrics = metrics;
            return this;
        }

        public GetChartRequest WithGrn(string grn) {
            this.Grn = grn;
            return this;
        }

        public GetChartRequest WithQueries(string[] queries) {
            this.Queries = queries;
            return this;
        }

        public GetChartRequest WithBy(string by) {
            this.By = by;
            return this;
        }

        public GetChartRequest WithTimeframe(string timeframe) {
            this.Timeframe = timeframe;
            return this;
        }

        public GetChartRequest WithSize(string size) {
            this.Size = size;
            return this;
        }

        public GetChartRequest WithFormat(string format) {
            this.Format = format;
            return this;
        }

        public GetChartRequest WithAggregator(string aggregator) {
            this.Aggregator = aggregator;
            return this;
        }

        public GetChartRequest WithStyle(string style) {
            this.Style = style;
            return this;
        }

        public GetChartRequest WithTitle(string title) {
            this.Title = title;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static GetChartRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new GetChartRequest()
                .WithMetrics(!data.Keys.Contains("metrics") || data["metrics"] == null ? null : data["metrics"].ToString())
                .WithGrn(!data.Keys.Contains("grn") || data["grn"] == null ? null : data["grn"].ToString())
                .WithQueries(!data.Keys.Contains("queries") || data["queries"] == null ? new string[]{} : data["queries"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithBy(!data.Keys.Contains("by") || data["by"] == null ? null : data["by"].ToString())
                .WithTimeframe(!data.Keys.Contains("timeframe") || data["timeframe"] == null ? null : data["timeframe"].ToString())
                .WithSize(!data.Keys.Contains("size") || data["size"] == null ? null : data["size"].ToString())
                .WithFormat(!data.Keys.Contains("format") || data["format"] == null ? null : data["format"].ToString())
                .WithAggregator(!data.Keys.Contains("aggregator") || data["aggregator"] == null ? null : data["aggregator"].ToString())
                .WithStyle(!data.Keys.Contains("style") || data["style"] == null ? null : data["style"].ToString())
                .WithTitle(!data.Keys.Contains("title") || data["title"] == null ? null : data["title"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["metrics"] = Metrics,
                ["grn"] = Grn,
                ["queries"] = new JsonData(Queries == null ? new JsonData[]{} :
                        Queries.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["by"] = By,
                ["timeframe"] = Timeframe,
                ["size"] = Size,
                ["format"] = Format,
                ["aggregator"] = Aggregator,
                ["style"] = Style,
                ["title"] = Title,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Metrics != null) {
                writer.WritePropertyName("metrics");
                writer.Write(Metrics.ToString());
            }
            if (Grn != null) {
                writer.WritePropertyName("grn");
                writer.Write(Grn.ToString());
            }
            writer.WriteArrayStart();
            foreach (var query in Queries)
            {
                writer.Write(query.ToString());
            }
            writer.WriteArrayEnd();
            if (By != null) {
                writer.WritePropertyName("by");
                writer.Write(By.ToString());
            }
            if (Timeframe != null) {
                writer.WritePropertyName("timeframe");
                writer.Write(Timeframe.ToString());
            }
            if (Size != null) {
                writer.WritePropertyName("size");
                writer.Write(Size.ToString());
            }
            if (Format != null) {
                writer.WritePropertyName("format");
                writer.Write(Format.ToString());
            }
            if (Aggregator != null) {
                writer.WritePropertyName("aggregator");
                writer.Write(Aggregator.ToString());
            }
            if (Style != null) {
                writer.WritePropertyName("style");
                writer.Write(Style.ToString());
            }
            if (Title != null) {
                writer.WritePropertyName("title");
                writer.Write(Title.ToString());
            }
            writer.WriteObjectEnd();
        }
    }
}