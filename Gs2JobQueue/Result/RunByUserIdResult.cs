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

namespace Gs2.Gs2JobQueue.Result
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class RunByUserIdResult : IResult
	{
        public Gs2.Gs2JobQueue.Model.Job Item { set; get; } = null!;
        public Gs2.Gs2JobQueue.Model.JobResultBody Result { set; get; } = null!;
        public bool? IsLastJob { set; get; } = null!;

        public RunByUserIdResult WithItem(Gs2.Gs2JobQueue.Model.Job item) {
            this.Item = item;
            return this;
        }

        public RunByUserIdResult WithResult(Gs2.Gs2JobQueue.Model.JobResultBody result) {
            this.Result = result;
            return this;
        }

        public RunByUserIdResult WithIsLastJob(bool? isLastJob) {
            this.IsLastJob = isLastJob;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunByUserIdResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunByUserIdResult()
                .WithItem(!data.Keys.Contains("item") || data["item"] == null ? null : Gs2.Gs2JobQueue.Model.Job.FromJson(data["item"]))
                .WithResult(!data.Keys.Contains("result") || data["result"] == null ? null : Gs2.Gs2JobQueue.Model.JobResultBody.FromJson(data["result"]))
                .WithIsLastJob(!data.Keys.Contains("isLastJob") || data["isLastJob"] == null ? null : (bool?)bool.Parse(data["isLastJob"].ToString()));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["item"] = Item?.ToJson(),
                ["result"] = Result?.ToJson(),
                ["isLastJob"] = IsLastJob,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Item != null) {
                Item.WriteJson(writer);
            }
            if (Result != null) {
                Result.WriteJson(writer);
            }
            if (IsLastJob != null) {
                writer.WritePropertyName("isLastJob");
                writer.Write(bool.Parse(IsLastJob.ToString()));
            }
            writer.WriteObjectEnd();
        }
    }
}