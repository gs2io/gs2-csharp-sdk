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
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Dictionary.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Dictionary.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteEntriesByUserIdRequest : Gs2Request<DeleteEntriesByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string[] EntryModelNames { set; get; }
        public string DuplicationAvoider { set; get; }
        public DeleteEntriesByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public DeleteEntriesByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public DeleteEntriesByUserIdRequest WithEntryModelNames(string[] entryModelNames) {
            this.EntryModelNames = entryModelNames;
            return this;
        }

        public DeleteEntriesByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteEntriesByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteEntriesByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithEntryModelNames(!data.Keys.Contains("entryModelNames") || data["entryModelNames"] == null ? new string[]{} : data["entryModelNames"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData entryModelNamesJsonData = null;
            if (EntryModelNames != null)
            {
                entryModelNamesJsonData = new JsonData();
                foreach (var entryModelName in EntryModelNames)
                {
                    entryModelNamesJsonData.Add(entryModelName);
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["entryModelNames"] = entryModelNamesJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var entryModelName in EntryModelNames)
            {
                writer.Write(entryModelName.ToString());
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += EntryModelNames + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new DeleteEntriesByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                EntryModelNames = EntryModelNames,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (DeleteEntriesByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values DeleteEntriesByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values DeleteEntriesByUserIdRequest::userId");
            }
            if (EntryModelNames != y.EntryModelNames) {
                throw new ArithmeticException("mismatch parameter values DeleteEntriesByUserIdRequest::entryModelNames");
            }
            return new DeleteEntriesByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                EntryModelNames = EntryModelNames,
            };
        }
    }
}