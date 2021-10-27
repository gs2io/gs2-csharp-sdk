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

namespace Gs2.Gs2Log.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class IssueStampSheetLogCount : IComparable
	{
        public string Service { set; get; }
        public string Method { set; get; }
        public string UserId { set; get; }
        public string Action { set; get; }
        public long? Count { set; get; }

        public IssueStampSheetLogCount WithService(string service) {
            this.Service = service;
            return this;
        }

        public IssueStampSheetLogCount WithMethod(string method) {
            this.Method = method;
            return this;
        }

        public IssueStampSheetLogCount WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public IssueStampSheetLogCount WithAction(string action) {
            this.Action = action;
            return this;
        }

        public IssueStampSheetLogCount WithCount(long? count) {
            this.Count = count;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static IssueStampSheetLogCount FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new IssueStampSheetLogCount()
                .WithService(!data.Keys.Contains("service") || data["service"] == null ? null : data["service"].ToString())
                .WithMethod(!data.Keys.Contains("method") || data["method"] == null ? null : data["method"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithAction(!data.Keys.Contains("action") || data["action"] == null ? null : data["action"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (long?)long.Parse(data["count"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["service"] = Service,
                ["method"] = Method,
                ["userId"] = UserId,
                ["action"] = Action,
                ["count"] = Count,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Service != null) {
                writer.WritePropertyName("service");
                writer.Write(Service.ToString());
            }
            if (Method != null) {
                writer.WritePropertyName("method");
                writer.Write(Method.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (Action != null) {
                writer.WritePropertyName("action");
                writer.Write(Action.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write(long.Parse(Count.ToString()));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as IssueStampSheetLogCount;
            var diff = 0;
            if (Service == null && Service == other.Service)
            {
                // null and null
            }
            else
            {
                diff += Service.CompareTo(other.Service);
            }
            if (Method == null && Method == other.Method)
            {
                // null and null
            }
            else
            {
                diff += Method.CompareTo(other.Method);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (Action == null && Action == other.Action)
            {
                // null and null
            }
            else
            {
                diff += Action.CompareTo(other.Action);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            return diff;
        }
    }
}