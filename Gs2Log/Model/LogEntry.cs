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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class LogEntry : IComparable
	{
        public long? Timestamp { set; get; }
        public string Status { set; get; }
        public long? Duration { set; get; }
        public string Line { set; get; }
        public Gs2.Gs2Log.Model.Label[] Labels { set; get; }
        public LogEntry WithTimestamp(long? timestamp) {
            this.Timestamp = timestamp;
            return this;
        }
        public LogEntry WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public LogEntry WithDuration(long? duration) {
            this.Duration = duration;
            return this;
        }
        public LogEntry WithLine(string line) {
            this.Line = line;
            return this;
        }
        public LogEntry WithLabels(Gs2.Gs2Log.Model.Label[] labels) {
            this.Labels = labels;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static LogEntry FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new LogEntry()
                .WithTimestamp(!data.Keys.Contains("timestamp") || data["timestamp"] == null ? null : (long?)(data["timestamp"].ToString().Contains(".") ? (long)double.Parse(data["timestamp"].ToString()) : long.Parse(data["timestamp"].ToString())))
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithDuration(!data.Keys.Contains("duration") || data["duration"] == null ? null : (long?)(data["duration"].ToString().Contains(".") ? (long)double.Parse(data["duration"].ToString()) : long.Parse(data["duration"].ToString())))
                .WithLine(!data.Keys.Contains("line") || data["line"] == null ? null : data["line"].ToString())
                .WithLabels(!data.Keys.Contains("labels") || data["labels"] == null || !data["labels"].IsArray ? null : data["labels"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Log.Model.Label.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData labelsJsonData = null;
            if (Labels != null && Labels.Length > 0)
            {
                labelsJsonData = new JsonData();
                foreach (var label in Labels)
                {
                    labelsJsonData.Add(label.ToJson());
                }
            }
            return new JsonData {
                ["timestamp"] = Timestamp,
                ["status"] = Status,
                ["duration"] = Duration,
                ["line"] = Line,
                ["labels"] = labelsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Timestamp != null) {
                writer.WritePropertyName("timestamp");
                writer.Write((Timestamp.ToString().Contains(".") ? (long)double.Parse(Timestamp.ToString()) : long.Parse(Timestamp.ToString())));
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (Duration != null) {
                writer.WritePropertyName("duration");
                writer.Write((Duration.ToString().Contains(".") ? (long)double.Parse(Duration.ToString()) : long.Parse(Duration.ToString())));
            }
            if (Line != null) {
                writer.WritePropertyName("line");
                writer.Write(Line.ToString());
            }
            if (Labels != null) {
                writer.WritePropertyName("labels");
                writer.WriteArrayStart();
                foreach (var label in Labels)
                {
                    if (label != null) {
                        label.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as LogEntry;
            var diff = 0;
            if (Timestamp == null && Timestamp == other.Timestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(Timestamp - other.Timestamp);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (Duration == null && Duration == other.Duration)
            {
                // null and null
            }
            else
            {
                diff += (int)(Duration - other.Duration);
            }
            if (Line == null && Line == other.Line)
            {
                // null and null
            }
            else
            {
                diff += Line.CompareTo(other.Line);
            }
            if (Labels == null && Labels == other.Labels)
            {
                // null and null
            }
            else
            {
                diff += Labels.Length - other.Labels.Length;
                for (var i = 0; i < Labels.Length; i++)
                {
                    diff += Labels[i].CompareTo(other.Labels[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (Timestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.timestamp.error.invalid"),
                    });
                }
                if (Timestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.timestamp.error.invalid"),
                    });
                }
            }
            {
                switch (Status) {
                    case "ok":
                    case "info":
                    case "notice":
                    case "error":
                    case "warn":
                    case "emag":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("logEntry", "log.logEntry.status.error.invalid"),
                        });
                }
            }
            {
                if (Duration < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.duration.error.invalid"),
                    });
                }
                if (Duration > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.duration.error.invalid"),
                    });
                }
            }
            {
                if (Line.Length > 10485760) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.line.error.tooLong"),
                    });
                }
            }
            {
                if (Labels.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("logEntry", "log.logEntry.labels.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new LogEntry {
                Timestamp = Timestamp,
                Status = Status,
                Duration = Duration,
                Line = Line,
                Labels = Labels?.Clone() as Gs2.Gs2Log.Model.Label[],
            };
        }
    }
}