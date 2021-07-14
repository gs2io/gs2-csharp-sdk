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

namespace Gs2.Gs2Mission.Model
{

	[Preserve]
	public class CounterModel : IComparable
	{
        public string CounterId { set; get; }
        public string Name { set; get; }
        public string Metadata { set; get; }
        public Gs2.Gs2Mission.Model.CounterScopeModel[] Scopes { set; get; }
        public string ChallengePeriodEventId { set; get; }

        public CounterModel WithCounterId(string counterId) {
            this.CounterId = counterId;
            return this;
        }

        public CounterModel WithName(string name) {
            this.Name = name;
            return this;
        }

        public CounterModel WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }

        public CounterModel WithScopes(Gs2.Gs2Mission.Model.CounterScopeModel[] scopes) {
            this.Scopes = scopes;
            return this;
        }

        public CounterModel WithChallengePeriodEventId(string challengePeriodEventId) {
            this.ChallengePeriodEventId = challengePeriodEventId;
            return this;
        }

    	[Preserve]
        public static CounterModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CounterModel()
                .WithCounterId(!data.Keys.Contains("counterId") || data["counterId"] == null ? null : data["counterId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithScopes(!data.Keys.Contains("scopes") || data["scopes"] == null ? new Gs2.Gs2Mission.Model.CounterScopeModel[]{} : data["scopes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Mission.Model.CounterScopeModel.FromJson(v);
                }).ToArray())
                .WithChallengePeriodEventId(!data.Keys.Contains("challengePeriodEventId") || data["challengePeriodEventId"] == null ? null : data["challengePeriodEventId"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["counterId"] = CounterId,
                ["name"] = Name,
                ["metadata"] = Metadata,
                ["scopes"] = new JsonData(Scopes == null ? new JsonData[]{} :
                        Scopes.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
                ["challengePeriodEventId"] = ChallengePeriodEventId,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (CounterId != null) {
                writer.WritePropertyName("counterId");
                writer.Write(CounterId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (Scopes != null) {
                writer.WritePropertyName("scopes");
                writer.WriteArrayStart();
                foreach (var scope in Scopes)
                {
                    if (scope != null) {
                        scope.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (ChallengePeriodEventId != null) {
                writer.WritePropertyName("challengePeriodEventId");
                writer.Write(ChallengePeriodEventId.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CounterModel;
            var diff = 0;
            if (CounterId == null && CounterId == other.CounterId)
            {
                // null and null
            }
            else
            {
                diff += CounterId.CompareTo(other.CounterId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Metadata == null && Metadata == other.Metadata)
            {
                // null and null
            }
            else
            {
                diff += Metadata.CompareTo(other.Metadata);
            }
            if (Scopes == null && Scopes == other.Scopes)
            {
                // null and null
            }
            else
            {
                diff += Scopes.Length - other.Scopes.Length;
                for (var i = 0; i < Scopes.Length; i++)
                {
                    diff += Scopes[i].CompareTo(other.Scopes[i]);
                }
            }
            if (ChallengePeriodEventId == null && ChallengePeriodEventId == other.ChallengePeriodEventId)
            {
                // null and null
            }
            else
            {
                diff += ChallengePeriodEventId.CompareTo(other.ChallengePeriodEventId);
            }
            return diff;
        }
    }
}