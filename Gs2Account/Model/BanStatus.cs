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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BanStatus : IComparable
	{
        public string Name { set; get; } = null!;
        public string Reason { set; get; } = null!;
        public long? ReleaseTimestamp { set; get; } = null!;
        public BanStatus WithName(string name) {
            this.Name = name;
            return this;
        }
        public BanStatus WithReason(string reason) {
            this.Reason = reason;
            return this;
        }
        public BanStatus WithReleaseTimestamp(long? releaseTimestamp) {
            this.ReleaseTimestamp = releaseTimestamp;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BanStatus FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BanStatus()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithReason(!data.Keys.Contains("reason") || data["reason"] == null ? null : data["reason"].ToString())
                .WithReleaseTimestamp(!data.Keys.Contains("releaseTimestamp") || data["releaseTimestamp"] == null ? null : (long?)(data["releaseTimestamp"].ToString().Contains(".") ? (long)double.Parse(data["releaseTimestamp"].ToString()) : long.Parse(data["releaseTimestamp"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["reason"] = Reason,
                ["releaseTimestamp"] = ReleaseTimestamp,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Reason != null) {
                writer.WritePropertyName("reason");
                writer.Write(Reason.ToString());
            }
            if (ReleaseTimestamp != null) {
                writer.WritePropertyName("releaseTimestamp");
                writer.Write((ReleaseTimestamp.ToString().Contains(".") ? (long)double.Parse(ReleaseTimestamp.ToString()) : long.Parse(ReleaseTimestamp.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BanStatus;
            var diff = 0;
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Reason == null && Reason == other.Reason)
            {
                // null and null
            }
            else
            {
                diff += Reason.CompareTo(other.Reason);
            }
            if (ReleaseTimestamp == null && ReleaseTimestamp == other.ReleaseTimestamp)
            {
                // null and null
            }
            else
            {
                diff += (int)(ReleaseTimestamp - other.ReleaseTimestamp);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("banStatus", "account.banStatus.name.error.tooLong"),
                    });
                }
            }
            {
                if (Reason.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("banStatus", "account.banStatus.reason.error.tooLong"),
                    });
                }
            }
            {
                if (ReleaseTimestamp < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("banStatus", "account.banStatus.releaseTimestamp.error.invalid"),
                    });
                }
                if (ReleaseTimestamp > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("banStatus", "account.banStatus.releaseTimestamp.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new BanStatus {
                Name = Name,
                Reason = Reason,
                ReleaseTimestamp = ReleaseTimestamp,
            };
        }
    }
}