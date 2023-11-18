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

namespace Gs2.Gs2Distributor.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class StampSheetResult : IComparable
	{
        public string StampSheetResultId { set; get; }
        public string UserId { set; get; }
        public string TransactionId { set; get; }
        public Gs2.Core.Model.ConsumeAction[] TaskRequests { set; get; }
        public Gs2.Core.Model.AcquireAction SheetRequest { set; get; }
        public string[] TaskResults { set; get; }
        public string SheetResult { set; get; }
        public string NextTransactionId { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }

        public StampSheetResult WithStampSheetResultId(string stampSheetResultId) {
            this.StampSheetResultId = stampSheetResultId;
            return this;
        }

        public StampSheetResult WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public StampSheetResult WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }

        public StampSheetResult WithTaskRequests(Gs2.Core.Model.ConsumeAction[] taskRequests) {
            this.TaskRequests = taskRequests;
            return this;
        }

        public StampSheetResult WithSheetRequest(Gs2.Core.Model.AcquireAction sheetRequest) {
            this.SheetRequest = sheetRequest;
            return this;
        }

        public StampSheetResult WithTaskResults(string[] taskResults) {
            this.TaskResults = taskResults;
            return this;
        }

        public StampSheetResult WithSheetResult(string sheetResult) {
            this.SheetResult = sheetResult;
            return this;
        }

        public StampSheetResult WithNextTransactionId(string nextTransactionId) {
            this.NextTransactionId = nextTransactionId;
            return this;
        }

        public StampSheetResult WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

        public StampSheetResult WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):stampSheet:result:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetRegionFromGrn(
            string grn
        )
        {
            var match = _regionRegex.Match(grn);
            if (!match.Success || !match.Groups["region"].Success)
            {
                return null;
            }
            return match.Groups["region"].Value;
        }

        private static System.Text.RegularExpressions.Regex _ownerIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):stampSheet:result:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetOwnerIdFromGrn(
            string grn
        )
        {
            var match = _ownerIdRegex.Match(grn);
            if (!match.Success || !match.Groups["ownerId"].Success)
            {
                return null;
            }
            return match.Groups["ownerId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _namespaceNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):stampSheet:result:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetNamespaceNameFromGrn(
            string grn
        )
        {
            var match = _namespaceNameRegex.Match(grn);
            if (!match.Success || !match.Groups["namespaceName"].Success)
            {
                return null;
            }
            return match.Groups["namespaceName"].Value;
        }

        private static System.Text.RegularExpressions.Regex _userIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):stampSheet:result:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetUserIdFromGrn(
            string grn
        )
        {
            var match = _userIdRegex.Match(grn);
            if (!match.Success || !match.Groups["userId"].Success)
            {
                return null;
            }
            return match.Groups["userId"].Value;
        }

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):distributor:(?<namespaceName>.+):user:(?<userId>.+):stampSheet:result:(?<transactionId>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetTransactionIdFromGrn(
            string grn
        )
        {
            var match = _transactionIdRegex.Match(grn);
            if (!match.Success || !match.Groups["transactionId"].Success)
            {
                return null;
            }
            return match.Groups["transactionId"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static StampSheetResult FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new StampSheetResult()
                .WithStampSheetResultId(!data.Keys.Contains("stampSheetResultId") || data["stampSheetResultId"] == null ? null : data["stampSheetResultId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithTaskRequests(!data.Keys.Contains("taskRequests") || data["taskRequests"] == null || !data["taskRequests"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["taskRequests"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithSheetRequest(!data.Keys.Contains("sheetRequest") || data["sheetRequest"] == null ? null : Gs2.Core.Model.AcquireAction.FromJson(data["sheetRequest"]))
                .WithTaskResults(!data.Keys.Contains("taskResults") || data["taskResults"] == null || !data["taskResults"].IsArray ? new string[]{} : data["taskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSheetResult(!data.Keys.Contains("sheetResult") || data["sheetResult"] == null ? null : data["sheetResult"].ToString())
                .WithNextTransactionId(!data.Keys.Contains("nextTransactionId") || data["nextTransactionId"] == null ? null : data["nextTransactionId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData taskRequestsJsonData = null;
            if (TaskRequests != null && TaskRequests.Length > 0)
            {
                taskRequestsJsonData = new JsonData();
                foreach (var taskRequest in TaskRequests)
                {
                    taskRequestsJsonData.Add(taskRequest.ToJson());
                }
            }
            JsonData taskResultsJsonData = null;
            if (TaskResults != null && TaskResults.Length > 0)
            {
                taskResultsJsonData = new JsonData();
                foreach (var taskResult in TaskResults)
                {
                    taskResultsJsonData.Add(taskResult);
                }
            }
            return new JsonData {
                ["stampSheetResultId"] = StampSheetResultId,
                ["userId"] = UserId,
                ["transactionId"] = TransactionId,
                ["taskRequests"] = taskRequestsJsonData,
                ["sheetRequest"] = SheetRequest?.ToJson(),
                ["taskResults"] = taskResultsJsonData,
                ["sheetResult"] = SheetResult,
                ["nextTransactionId"] = NextTransactionId,
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (StampSheetResultId != null) {
                writer.WritePropertyName("stampSheetResultId");
                writer.Write(StampSheetResultId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (TaskRequests != null) {
                writer.WritePropertyName("taskRequests");
                writer.WriteArrayStart();
                foreach (var taskRequest in TaskRequests)
                {
                    if (taskRequest != null) {
                        taskRequest.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SheetRequest != null) {
                writer.WritePropertyName("sheetRequest");
                SheetRequest.WriteJson(writer);
            }
            if (TaskResults != null) {
                writer.WritePropertyName("taskResults");
                writer.WriteArrayStart();
                foreach (var taskResult in TaskResults)
                {
                    if (taskResult != null) {
                        writer.Write(taskResult.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (SheetResult != null) {
                writer.WritePropertyName("sheetResult");
                writer.Write(SheetResult.ToString());
            }
            if (NextTransactionId != null) {
                writer.WritePropertyName("nextTransactionId");
                writer.Write(NextTransactionId.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as StampSheetResult;
            var diff = 0;
            if (StampSheetResultId == null && StampSheetResultId == other.StampSheetResultId)
            {
                // null and null
            }
            else
            {
                diff += StampSheetResultId.CompareTo(other.StampSheetResultId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (TaskRequests == null && TaskRequests == other.TaskRequests)
            {
                // null and null
            }
            else
            {
                diff += TaskRequests.Length - other.TaskRequests.Length;
                for (var i = 0; i < TaskRequests.Length; i++)
                {
                    diff += TaskRequests[i].CompareTo(other.TaskRequests[i]);
                }
            }
            if (SheetRequest == null && SheetRequest == other.SheetRequest)
            {
                // null and null
            }
            else
            {
                diff += SheetRequest.CompareTo(other.SheetRequest);
            }
            if (TaskResults == null && TaskResults == other.TaskResults)
            {
                // null and null
            }
            else
            {
                diff += TaskResults.Length - other.TaskResults.Length;
                for (var i = 0; i < TaskResults.Length; i++)
                {
                    diff += TaskResults[i].CompareTo(other.TaskResults[i]);
                }
            }
            if (SheetResult == null && SheetResult == other.SheetResult)
            {
                // null and null
            }
            else
            {
                diff += SheetResult.CompareTo(other.SheetResult);
            }
            if (NextTransactionId == null && NextTransactionId == other.NextTransactionId)
            {
                // null and null
            }
            else
            {
                diff += NextTransactionId.CompareTo(other.NextTransactionId);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (Revision == null && Revision == other.Revision)
            {
                // null and null
            }
            else
            {
                diff += (int)(Revision - other.Revision);
            }
            return diff;
        }
    }
}