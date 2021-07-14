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
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Watch.Model
{

	[Preserve]
	public class Chart : IComparable
	{
        public string ChartId { set; get; }
        public string EmbedId { set; get; }
        public string Html { set; get; }

        public Chart WithChartId(string chartId) {
            this.ChartId = chartId;
            return this;
        }

        public Chart WithEmbedId(string embedId) {
            this.EmbedId = embedId;
            return this;
        }

        public Chart WithHtml(string html) {
            this.Html = html;
            return this;
        }

    	[Preserve]
        public static Chart FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Chart()
                .WithChartId(!data.Keys.Contains("chartId") || data["chartId"] == null ? null : data["chartId"].ToString())
                .WithEmbedId(!data.Keys.Contains("embedId") || data["embedId"] == null ? null : data["embedId"].ToString())
                .WithHtml(!data.Keys.Contains("html") || data["html"] == null ? null : data["html"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["chartId"] = ChartId,
                ["embedId"] = EmbedId,
                ["html"] = Html,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ChartId != null) {
                writer.WritePropertyName("chartId");
                writer.Write(ChartId.ToString());
            }
            if (EmbedId != null) {
                writer.WritePropertyName("embedId");
                writer.Write(EmbedId.ToString());
            }
            if (Html != null) {
                writer.WritePropertyName("html");
                writer.Write(Html.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Chart;
            var diff = 0;
            if (ChartId == null && ChartId == other.ChartId)
            {
                // null and null
            }
            else
            {
                diff += ChartId.CompareTo(other.ChartId);
            }
            if (EmbedId == null && EmbedId == other.EmbedId)
            {
                // null and null
            }
            else
            {
                diff += EmbedId.CompareTo(other.EmbedId);
            }
            if (Html == null && Html == other.Html)
            {
                // null and null
            }
            else
            {
                diff += Html.CompareTo(other.Html);
            }
            return diff;
        }
    }
}