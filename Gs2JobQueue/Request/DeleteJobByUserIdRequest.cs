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
using Gs2.Gs2JobQueue.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2JobQueue.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class DeleteJobByUserIdRequest : Gs2Request<DeleteJobByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string JobName { set; get; }
        public string DuplicationAvoider { set; get; }

        public DeleteJobByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public DeleteJobByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public DeleteJobByUserIdRequest WithJobName(string jobName) {
            this.JobName = jobName;
            return this;
        }

        public DeleteJobByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static DeleteJobByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new DeleteJobByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithJobName(!data.Keys.Contains("jobName") || data["jobName"] == null ? null : data["jobName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["jobName"] = JobName,
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
            if (JobName != null) {
                writer.WritePropertyName("jobName");
                writer.Write(JobName.ToString());
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += JobName + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new DeleteJobByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                JobName = JobName,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (DeleteJobByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values DeleteJobByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values DeleteJobByUserIdRequest::userId");
            }
            if (JobName != y.JobName) {
                throw new ArithmeticException("mismatch parameter values DeleteJobByUserIdRequest::jobName");
            }
            return new DeleteJobByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                JobName = JobName,
            };
        }
    }
}