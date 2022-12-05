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

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class CapacityOfRole : IComparable
	{
        public string RoleName { set; get; }
        public string[] RoleAliases { set; get; }
        public int? Capacity { set; get; }
        public Gs2.Gs2Matchmaking.Model.Player[] Participants { set; get; }
        public CapacityOfRole WithRoleName(string roleName) {
            this.RoleName = roleName;
            return this;
        }
        public CapacityOfRole WithRoleAliases(string[] roleAliases) {
            this.RoleAliases = roleAliases;
            return this;
        }
        public CapacityOfRole WithCapacity(int? capacity) {
            this.Capacity = capacity;
            return this;
        }
        public CapacityOfRole WithParticipants(Gs2.Gs2Matchmaking.Model.Player[] participants) {
            this.Participants = participants;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CapacityOfRole FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CapacityOfRole()
                .WithRoleName(!data.Keys.Contains("roleName") || data["roleName"] == null ? null : data["roleName"].ToString())
                .WithRoleAliases(!data.Keys.Contains("roleAliases") || data["roleAliases"] == null ? new string[]{} : data["roleAliases"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithCapacity(!data.Keys.Contains("capacity") || data["capacity"] == null ? null : (int?)int.Parse(data["capacity"].ToString()))
                .WithParticipants(!data.Keys.Contains("participants") || data["participants"] == null ? new Gs2.Gs2Matchmaking.Model.Player[]{} : data["participants"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Matchmaking.Model.Player.FromJson(v);
                }).ToArray());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["roleName"] = RoleName,
                ["roleAliases"] = RoleAliases == null ? null : new JsonData(
                        RoleAliases.Select(v => {
                            return new JsonData(v.ToString());
                        }).ToArray()
                    ),
                ["capacity"] = Capacity,
                ["participants"] = Participants == null ? null : new JsonData(
                        Participants.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (RoleName != null) {
                writer.WritePropertyName("roleName");
                writer.Write(RoleName.ToString());
            }
            if (RoleAliases != null) {
                writer.WritePropertyName("roleAliases");
                writer.WriteArrayStart();
                foreach (var roleAlias in RoleAliases)
                {
                    if (roleAlias != null) {
                        writer.Write(roleAlias.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Capacity != null) {
                writer.WritePropertyName("capacity");
                writer.Write(int.Parse(Capacity.ToString()));
            }
            if (Participants != null) {
                writer.WritePropertyName("participants");
                writer.WriteArrayStart();
                foreach (var participant in Participants)
                {
                    if (participant != null) {
                        participant.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as CapacityOfRole;
            var diff = 0;
            if (RoleName == null && RoleName == other.RoleName)
            {
                // null and null
            }
            else
            {
                diff += RoleName.CompareTo(other.RoleName);
            }
            if (RoleAliases == null && RoleAliases == other.RoleAliases)
            {
                // null and null
            }
            else
            {
                diff += RoleAliases.Length - other.RoleAliases.Length;
                for (var i = 0; i < RoleAliases.Length; i++)
                {
                    diff += RoleAliases[i].CompareTo(other.RoleAliases[i]);
                }
            }
            if (Capacity == null && Capacity == other.Capacity)
            {
                // null and null
            }
            else
            {
                diff += (int)(Capacity - other.Capacity);
            }
            if (Participants == null && Participants == other.Participants)
            {
                // null and null
            }
            else
            {
                diff += Participants.Length - other.Participants.Length;
                for (var i = 0; i < Participants.Length; i++)
                {
                    diff += Participants[i].CompareTo(other.Participants[i]);
                }
            }
            return diff;
        }
    }
}