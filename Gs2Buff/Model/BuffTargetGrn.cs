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

namespace Gs2.Gs2Buff.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class BuffTargetGrn : IComparable
	{
        public string TargetModelName { set; get; }
        public string TargetGrn { set; get; }
        public BuffTargetGrn WithTargetModelName(string targetModelName) {
            this.TargetModelName = targetModelName;
            return this;
        }
        public BuffTargetGrn WithTargetGrn(string targetGrn) {
            this.TargetGrn = targetGrn;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BuffTargetGrn FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                // The model arrived as the JSON text of itself rather than as
                // an object. A stamp sheet carries values the client supplied
                // as strings — a store receipt is one — so reading such a
                // request back finds the text where the object is expected.
                data = JsonMapper.ToObject(data.ToString());
            }
            return new BuffTargetGrn()
                .WithTargetModelName(!data.Keys.Contains("targetModelName") || data["targetModelName"] == null ? null : data["targetModelName"].ToString())
                .WithTargetGrn(!data.Keys.Contains("targetGrn") || data["targetGrn"] == null ? null : data["targetGrn"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["targetModelName"] = TargetModelName,
                ["targetGrn"] = TargetGrn,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (TargetModelName != null) {
                writer.WritePropertyName("targetModelName");
                writer.Write(TargetModelName.ToString());
            }
            if (TargetGrn != null) {
                writer.WritePropertyName("targetGrn");
                writer.Write(TargetGrn.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BuffTargetGrn;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type BuffTargetGrn.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(TargetModelName, other.TargetModelName);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TargetGrn, other.TargetGrn);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
                if (TargetModelName.Length > 64) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetGrn", "buff.buffTargetGrn.targetModelName.error.tooLong"),
                    });
                }
            }
            {
                if (TargetGrn.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("buffTargetGrn", "buff.buffTargetGrn.targetGrn.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new BuffTargetGrn {
                TargetModelName = TargetModelName,
                TargetGrn = TargetGrn,
            };
        }
    }
}