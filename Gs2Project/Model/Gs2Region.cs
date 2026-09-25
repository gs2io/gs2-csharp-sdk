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

namespace Gs2.Gs2Project.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class Gs2Region : IComparable
	{
        public string RegionName { set; get; }
        public string Status { set; get; }
        public Gs2Region WithRegionName(string regionName) {
            this.RegionName = regionName;
            return this;
        }
        public Gs2Region WithStatus(string status) {
            this.Status = status;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Gs2Region FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Gs2Region()
                .WithRegionName(!data.Keys.Contains("regionName") || data["regionName"] == null ? null : data["regionName"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["regionName"] = RegionName,
                ["status"] = Status,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RegionName != null) {
                writer.WritePropertyName("regionName");
                writer.Write(RegionName.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Gs2Region;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Gs2Region.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(RegionName, other.RegionName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Status, other.Status);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                switch (RegionName) {
                    case "ap-northeast-1":
                    case "us-east-1":
                    case "eu-west-1":
                    case "ap-southeast-1":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("gs2Region", "project.gs2Region.regionName.error.invalid"),
                        });
                }
            }
            {
                switch (Status) {
                    case "enabled":
                    case "pending":
                    case "suspended":
                    case "disabled":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("gs2Region", "project.gs2Region.status.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new Gs2Region {
                RegionName = RegionName,
                Status = Status,
            };
        }
    }
}