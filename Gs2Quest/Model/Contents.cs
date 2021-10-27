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
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Quest.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Contents : IComparable
	{
        public string Metadata { set; get; }
        public Gs2.Gs2Quest.Model.AcquireAction[] CompleteAcquireActions { set; get; }
        public int? Weight { set; get; }

        public Contents WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public Contents WithCompleteAcquireActions(Gs2.Gs2Quest.Model.AcquireAction[] completeAcquireActions) {
            this.CompleteAcquireActions = completeAcquireActions;
            return this;
        }

        public Contents WithWeight(int? weight) {
            this.Weight = weight;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Contents FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Contents()
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithCompleteAcquireActions(!data.Keys.Contains("completeAcquireActions") || data["completeAcquireActions"] == null ? new Gs2.Gs2Quest.Model.AcquireAction[]{} : data["completeAcquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Quest.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithWeight(!data.Keys.Contains("weight") || data["weight"] == null ? null : (int?)int.Parse(data["weight"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["metadata"] = Metadata,
                ["completeAcquireActions"] = new JsonData(CompleteAcquireActions == null ? new JsonData[]{} :
                        CompleteAcquireActions.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["weight"] = Weight,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (CompleteAcquireActions != null) {
                writer.WritePropertyName("completeAcquireActions");
                writer.WriteArrayStart();
                foreach (var completeAcquireAction in CompleteAcquireActions)
                {
                    if (completeAcquireAction != null) {
                        completeAcquireAction.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Weight != null) {
                writer.WritePropertyName("weight");
                writer.Write(int.Parse(Weight.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Contents;
            var diff = 0;
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (CompleteAcquireActions == null && CompleteAcquireActions == other.CompleteAcquireActions)
            {
                // null and null
            }
            else
            {
                diff += CompleteAcquireActions.Length - other.CompleteAcquireActions.Length;
                for (var i = 0; i < CompleteAcquireActions.Length; i++)
                {
                    diff += CompleteAcquireActions[i].CompareTo(other.CompleteAcquireActions[i]);
                }
            }
            if (Weight == null && Weight == other.Weight)
            {
                // null and null
            }
            else
            {
                diff += (int)(Weight - other.Weight);
            }
            return diff;
        }
    }
}