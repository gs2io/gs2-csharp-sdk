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

namespace Gs2.Gs2AdReward.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class AdMob : IComparable
	{
        public string[] AllowAdUnitIds { set; get; } = null!;
        public AdMob WithAllowAdUnitIds(string[] allowAdUnitIds) {
            this.AllowAdUnitIds = allowAdUnitIds;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AdMob FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AdMob()
                .WithAllowAdUnitIds(!data.Keys.Contains("allowAdUnitIds") || data["allowAdUnitIds"] == null || !data["allowAdUnitIds"].IsArray ? null : data["allowAdUnitIds"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData allowAdUnitIdsJsonData = null;
            if (AllowAdUnitIds != null && AllowAdUnitIds.Length > 0)
            {
                allowAdUnitIdsJsonData = new JsonData();
                foreach (var allowAdUnitId in AllowAdUnitIds)
                {
                    allowAdUnitIdsJsonData.Add(allowAdUnitId);
                }
            }
            return new JsonData {
                ["allowAdUnitIds"] = allowAdUnitIdsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AllowAdUnitIds != null) {
                writer.WritePropertyName("allowAdUnitIds");
                writer.WriteArrayStart();
                foreach (var allowAdUnitId in AllowAdUnitIds)
                {
                    if (allowAdUnitId != null) {
                        writer.Write(allowAdUnitId.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as AdMob;
            var diff = 0;
            if (AllowAdUnitIds == null && AllowAdUnitIds == other.AllowAdUnitIds)
            {
                // null and null
            }
            else
            {
                diff += AllowAdUnitIds.Length - other.AllowAdUnitIds.Length;
                for (var i = 0; i < AllowAdUnitIds.Length; i++)
                {
                    diff += AllowAdUnitIds[i].CompareTo(other.AllowAdUnitIds[i]);
                }
            }
            return diff;
        }

        public void Validate() {
            {
                if (AllowAdUnitIds.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("adMob", "adReward.adMob.allowAdUnitIds.error.tooFew"),
                    });
                }
                if (AllowAdUnitIds.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("adMob", "adReward.adMob.allowAdUnitIds.error.tooMany"),
                    });
                }
            }
        }

        public object Clone() {
            return new AdMob {
                AllowAdUnitIds = AllowAdUnitIds.Clone() as string[],
            };
        }
    }
}