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

namespace Gs2.Gs2SeasonRating.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class TierModel : IComparable
	{
        public string Metadata { set; get; } = null!;
        public int? RaiseRankBonus { set; get; } = null!;
        public int? EntryFee { set; get; } = null!;
        public int? MinimumChangePoint { set; get; } = null!;
        public int? MaximumChangePoint { set; get; } = null!;
        public TierModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public TierModel WithRaiseRankBonus(int? raiseRankBonus) {
            this.RaiseRankBonus = raiseRankBonus;
            return this;
        }
        public TierModel WithEntryFee(int? entryFee) {
            this.EntryFee = entryFee;
            return this;
        }
        public TierModel WithMinimumChangePoint(int? minimumChangePoint) {
            this.MinimumChangePoint = minimumChangePoint;
            return this;
        }
        public TierModel WithMaximumChangePoint(int? maximumChangePoint) {
            this.MaximumChangePoint = maximumChangePoint;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static TierModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new TierModel()
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithRaiseRankBonus(!data.Keys.Contains("raiseRankBonus") || data["raiseRankBonus"] == null ? null : (int?)(data["raiseRankBonus"].ToString().Contains(".") ? (int)double.Parse(data["raiseRankBonus"].ToString()) : int.Parse(data["raiseRankBonus"].ToString())))
                .WithEntryFee(!data.Keys.Contains("entryFee") || data["entryFee"] == null ? null : (int?)(data["entryFee"].ToString().Contains(".") ? (int)double.Parse(data["entryFee"].ToString()) : int.Parse(data["entryFee"].ToString())))
                .WithMinimumChangePoint(!data.Keys.Contains("minimumChangePoint") || data["minimumChangePoint"] == null ? null : (int?)(data["minimumChangePoint"].ToString().Contains(".") ? (int)double.Parse(data["minimumChangePoint"].ToString()) : int.Parse(data["minimumChangePoint"].ToString())))
                .WithMaximumChangePoint(!data.Keys.Contains("maximumChangePoint") || data["maximumChangePoint"] == null ? null : (int?)(data["maximumChangePoint"].ToString().Contains(".") ? (int)double.Parse(data["maximumChangePoint"].ToString()) : int.Parse(data["maximumChangePoint"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["metadata"] = Metadata,
                ["raiseRankBonus"] = RaiseRankBonus,
                ["entryFee"] = EntryFee,
                ["minimumChangePoint"] = MinimumChangePoint,
                ["maximumChangePoint"] = MaximumChangePoint,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (RaiseRankBonus != null) {
                writer.WritePropertyName("raiseRankBonus");
                writer.Write((RaiseRankBonus.ToString().Contains(".") ? (int)double.Parse(RaiseRankBonus.ToString()) : int.Parse(RaiseRankBonus.ToString())));
            }
            if (EntryFee != null) {
                writer.WritePropertyName("entryFee");
                writer.Write((EntryFee.ToString().Contains(".") ? (int)double.Parse(EntryFee.ToString()) : int.Parse(EntryFee.ToString())));
            }
            if (MinimumChangePoint != null) {
                writer.WritePropertyName("minimumChangePoint");
                writer.Write((MinimumChangePoint.ToString().Contains(".") ? (int)double.Parse(MinimumChangePoint.ToString()) : int.Parse(MinimumChangePoint.ToString())));
            }
            if (MaximumChangePoint != null) {
                writer.WritePropertyName("maximumChangePoint");
                writer.Write((MaximumChangePoint.ToString().Contains(".") ? (int)double.Parse(MaximumChangePoint.ToString()) : int.Parse(MaximumChangePoint.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as TierModel;
            var diff = 0;
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (RaiseRankBonus == null && RaiseRankBonus == other.RaiseRankBonus)
            {
                // null and null
            }
            else
            {
                diff += (int)(RaiseRankBonus - other.RaiseRankBonus);
            }
            if (EntryFee == null && EntryFee == other.EntryFee)
            {
                // null and null
            }
            else
            {
                diff += (int)(EntryFee - other.EntryFee);
            }
            if (MinimumChangePoint == null && MinimumChangePoint == other.MinimumChangePoint)
            {
                // null and null
            }
            else
            {
                diff += (int)(MinimumChangePoint - other.MinimumChangePoint);
            }
            if (MaximumChangePoint == null && MaximumChangePoint == other.MaximumChangePoint)
            {
                // null and null
            }
            else
            {
                diff += (int)(MaximumChangePoint - other.MaximumChangePoint);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Metadata.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.metadata.error.tooLong"),
                    });
                }
            }
            {
                if (RaiseRankBonus < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.raiseRankBonus.error.invalid"),
                    });
                }
                if (RaiseRankBonus > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.raiseRankBonus.error.invalid"),
                    });
                }
            }
            {
                if (EntryFee < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.entryFee.error.invalid"),
                    });
                }
                if (EntryFee > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.entryFee.error.invalid"),
                    });
                }
            }
            {
                if (MinimumChangePoint < -99999999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.minimumChangePoint.error.invalid"),
                    });
                }
                if (MinimumChangePoint > -1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.minimumChangePoint.error.invalid"),
                    });
                }
            }
            {
                if (MaximumChangePoint < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.maximumChangePoint.error.invalid"),
                    });
                }
                if (MaximumChangePoint > 99999999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("tierModel", "seasonRating.tierModel.maximumChangePoint.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new TierModel {
                Metadata = Metadata,
                RaiseRankBonus = RaiseRankBonus,
                EntryFee = EntryFee,
                MinimumChangePoint = MinimumChangePoint,
                MaximumChangePoint = MaximumChangePoint,
            };
        }
    }
}