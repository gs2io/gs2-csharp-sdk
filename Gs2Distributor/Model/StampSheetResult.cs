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
 *
 * deny overwrite
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Core.Util;
using Gs2.Gs2JobQueue.Model;
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
        public string StampSheetResultId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public Gs2.Core.Model.VerifyAction[] VerifyTaskRequests { set; get; } = null!;
        public Gs2.Core.Model.ConsumeAction[] TaskRequests { set; get; } = null!;
        public Gs2.Core.Model.AcquireAction SheetRequest { set; get; } = null!;
        public int[] VerifyTaskResultCodes { set; get; } = null!;
        public string[] VerifyTaskResults { set; get; } = null!;
        public int[] TaskResultCodes { set; get; } = null!;
        public string[] TaskResults { set; get; } = null!;
        public int? SheetResultCode { set; get; } = null!;
        public string SheetResult { set; get; } = null!;
        public string NextTransactionId { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
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
        public StampSheetResult WithVerifyTaskRequests(Gs2.Core.Model.VerifyAction[] verifyTaskRequests) {
            this.VerifyTaskRequests = verifyTaskRequests;
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
        public StampSheetResult WithVerifyTaskResultCodes(int[] verifyTaskResultCodes) {
            this.VerifyTaskResultCodes = verifyTaskResultCodes;
            return this;
        }
        public StampSheetResult WithVerifyTaskResults(string[] verifyTaskResults) {
            this.VerifyTaskResults = verifyTaskResults;
            return this;
        }
        public StampSheetResult WithTaskResultCodes(int[] taskResultCodes) {
            this.TaskResultCodes = taskResultCodes;
            return this;
        }
        public StampSheetResult WithTaskResults(string[] taskResults) {
            this.TaskResults = taskResults;
            return this;
        }
        public StampSheetResult WithSheetResultCode(int? sheetResultCode) {
            this.SheetResultCode = sheetResultCode;
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
            var result = new StampSheetResult()
                .WithStampSheetResultId(!data.Keys.Contains("stampSheetResultId") || data["stampSheetResultId"] == null ? null : data["stampSheetResultId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithVerifyTaskRequests(!data.Keys.Contains("verifyTaskRequests") || data["verifyTaskRequests"] == null || !data["verifyTaskRequests"].IsArray ? new Gs2.Core.Model.VerifyAction[]{} : data["verifyTaskRequests"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.VerifyAction.FromJson(v);
                }).ToArray())
                .WithTaskRequests(!data.Keys.Contains("taskRequests") || data["taskRequests"] == null || !data["taskRequests"].IsArray ? new Gs2.Core.Model.ConsumeAction[]{} : data["taskRequests"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithSheetRequest(!data.Keys.Contains("sheetRequest") || data["sheetRequest"] == null ? null : Gs2.Core.Model.AcquireAction.FromJson(data["sheetRequest"]))
                .WithVerifyTaskResultCodes(!data.Keys.Contains("verifyTaskResultCodes") || data["verifyTaskResultCodes"] == null || !data["verifyTaskResultCodes"].IsArray ? new int[]{} : data["verifyTaskResultCodes"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithVerifyTaskResults(!data.Keys.Contains("verifyTaskResults") || data["verifyTaskResults"] == null || !data["verifyTaskResults"].IsArray ? new string[]{} : data["verifyTaskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithTaskResultCodes(!data.Keys.Contains("taskResultCodes") || data["taskResultCodes"] == null || !data["taskResultCodes"].IsArray ? new int[]{} : data["taskResultCodes"].Cast<JsonData>().Select(v => {
                    return (v.ToString().Contains(".") ? (int)double.Parse(v.ToString()) : int.Parse(v.ToString()));
                }).ToArray())
                .WithTaskResults(!data.Keys.Contains("taskResults") || data["taskResults"] == null || !data["taskResults"].IsArray ? new string[]{} : data["taskResults"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithSheetResultCode(!data.Keys.Contains("sheetResultCode") || data["sheetResultCode"] == null ? null : (int?)(data["sheetResultCode"].ToString().Contains(".") ? (int)double.Parse(data["sheetResultCode"].ToString()) : int.Parse(data["sheetResultCode"].ToString())))
                .WithSheetResult(!data.Keys.Contains("sheetResult") || data["sheetResult"] == null ? null : data["sheetResult"].ToString())
                .WithNextTransactionId(!data.Keys.Contains("nextTransactionId") || data["nextTransactionId"] == null ? null : data["nextTransactionId"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
            
            if (result != null) {
                Telemetry.HandleTransaction(result.TransactionId, result);
            }

            return result;
        }

        public JsonData ToJson()
        {
            JsonData verifyTaskRequestsJsonData = null;
            if (VerifyTaskRequests != null && VerifyTaskRequests.Length > 0)
            {
                verifyTaskRequestsJsonData = new JsonData();
                foreach (var verifyTaskRequest in VerifyTaskRequests)
                {
                    verifyTaskRequestsJsonData.Add(verifyTaskRequest.ToJson());
                }
            }
            JsonData taskRequestsJsonData = null;
            if (TaskRequests != null && TaskRequests.Length > 0)
            {
                taskRequestsJsonData = new JsonData();
                foreach (var taskRequest in TaskRequests)
                {
                    taskRequestsJsonData.Add(taskRequest.ToJson());
                }
            }
            JsonData verifyTaskResultCodesJsonData = null;
            if (VerifyTaskResultCodes != null && VerifyTaskResultCodes.Length > 0)
            {
                verifyTaskResultCodesJsonData = new JsonData();
                foreach (var verifyTaskResultCode in VerifyTaskResultCodes)
                {
                    verifyTaskResultCodesJsonData.Add(verifyTaskResultCode);
                }
            }
            JsonData verifyTaskResultsJsonData = null;
            if (VerifyTaskResults != null && VerifyTaskResults.Length > 0)
            {
                verifyTaskResultsJsonData = new JsonData();
                foreach (var verifyTaskResult in VerifyTaskResults)
                {
                    verifyTaskResultsJsonData.Add(verifyTaskResult);
                }
            }
            JsonData taskResultCodesJsonData = null;
            if (TaskResultCodes != null && TaskResultCodes.Length > 0)
            {
                taskResultCodesJsonData = new JsonData();
                foreach (var taskResultCode in TaskResultCodes)
                {
                    taskResultCodesJsonData.Add(taskResultCode);
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
                ["verifyTaskRequests"] = verifyTaskRequestsJsonData,
                ["taskRequests"] = taskRequestsJsonData,
                ["sheetRequest"] = SheetRequest?.ToJson(),
                ["verifyTaskResultCodes"] = verifyTaskResultCodesJsonData,
                ["verifyTaskResults"] = verifyTaskResultsJsonData,
                ["taskResultCodes"] = taskResultCodesJsonData,
                ["taskResults"] = taskResultsJsonData,
                ["sheetResultCode"] = SheetResultCode,
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
            if (VerifyTaskRequests != null) {
                writer.WritePropertyName("verifyTaskRequests");
                writer.WriteArrayStart();
                foreach (var verifyTaskRequest in VerifyTaskRequests)
                {
                    if (verifyTaskRequest != null) {
                        verifyTaskRequest.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
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
            if (VerifyTaskResultCodes != null) {
                writer.WritePropertyName("verifyTaskResultCodes");
                writer.WriteArrayStart();
                foreach (var verifyTaskResultCode in VerifyTaskResultCodes)
                {
                    writer.Write((verifyTaskResultCode.ToString().Contains(".") ? (int)double.Parse(verifyTaskResultCode.ToString()) : int.Parse(verifyTaskResultCode.ToString())));
                }
                writer.WriteArrayEnd();
            }
            if (VerifyTaskResults != null) {
                writer.WritePropertyName("verifyTaskResults");
                writer.WriteArrayStart();
                foreach (var verifyTaskResult in VerifyTaskResults)
                {
                    if (verifyTaskResult != null) {
                        writer.Write(verifyTaskResult.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (TaskResultCodes != null) {
                writer.WritePropertyName("taskResultCodes");
                writer.WriteArrayStart();
                foreach (var taskResultCode in TaskResultCodes)
                {
                    writer.Write((taskResultCode.ToString().Contains(".") ? (int)double.Parse(taskResultCode.ToString()) : int.Parse(taskResultCode.ToString())));
                }
                writer.WriteArrayEnd();
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
            if (SheetResultCode != null) {
                writer.WritePropertyName("sheetResultCode");
                writer.Write((SheetResultCode.ToString().Contains(".") ? (int)double.Parse(SheetResultCode.ToString()) : int.Parse(SheetResultCode.ToString())));
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
            if (VerifyTaskRequests == null && VerifyTaskRequests == other.VerifyTaskRequests)
            {
                // null and null
            }
            else
            {
                diff += VerifyTaskRequests.Length - other.VerifyTaskRequests.Length;
                for (var i = 0; i < VerifyTaskRequests.Length; i++)
                {
                    diff += VerifyTaskRequests[i].CompareTo(other.VerifyTaskRequests[i]);
                }
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
            if (VerifyTaskResultCodes == null && VerifyTaskResultCodes == other.VerifyTaskResultCodes)
            {
                // null and null
            }
            else
            {
                diff += VerifyTaskResultCodes.Length - other.VerifyTaskResultCodes.Length;
                for (var i = 0; i < VerifyTaskResultCodes.Length; i++)
                {
                    diff += (int)(VerifyTaskResultCodes[i] - other.VerifyTaskResultCodes[i]);
                }
            }
            if (VerifyTaskResults == null && VerifyTaskResults == other.VerifyTaskResults)
            {
                // null and null
            }
            else
            {
                diff += VerifyTaskResults.Length - other.VerifyTaskResults.Length;
                for (var i = 0; i < VerifyTaskResults.Length; i++)
                {
                    diff += VerifyTaskResults[i].CompareTo(other.VerifyTaskResults[i]);
                }
            }
            if (TaskResultCodes == null && TaskResultCodes == other.TaskResultCodes)
            {
                // null and null
            }
            else
            {
                diff += TaskResultCodes.Length - other.TaskResultCodes.Length;
                for (var i = 0; i < TaskResultCodes.Length; i++)
                {
                    diff += (int)(TaskResultCodes[i] - other.TaskResultCodes[i]);
                }
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
            if (SheetResultCode == null && SheetResultCode == other.SheetResultCode)
            {
                // null and null
            }
            else
            {
                diff += (int)(SheetResultCode - other.SheetResultCode);
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

        public void Validate() {
            {
                if (StampSheetResultId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.stampSheetResultId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.userId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length < 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.transactionId.error.tooShort"),
                    });
                }
                if (TransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (VerifyTaskRequests.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.verifyTaskRequests.error.tooMany"),
                    });
                }
            }
            {
                if (TaskRequests.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.taskRequests.error.tooMany"),
                    });
                }
            }
            {
            }
            {
                if (VerifyTaskResultCodes.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.verifyTaskResultCodes.error.tooMany"),
                    });
                }
            }
            {
                if (VerifyTaskResults.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.verifyTaskResults.error.tooMany"),
                    });
                }
            }
            {
                if (TaskResultCodes.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.taskResultCodes.error.tooMany"),
                    });
                }
            }
            {
                if (TaskResults.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.taskResults.error.tooMany"),
                    });
                }
            }
            {
                if (SheetResultCode < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.sheetResultCode.error.invalid"),
                    });
                }
                if (SheetResultCode > 999) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.sheetResultCode.error.invalid"),
                    });
                }
            }
            {
                if (SheetResult.Length > 1048576) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.sheetResult.error.tooLong"),
                    });
                }
            }
            {
                if (NextTransactionId.Length < 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.nextTransactionId.error.tooShort"),
                    });
                }
                if (NextTransactionId.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.nextTransactionId.error.tooLong"),
                    });
                }
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("stampSheetResult", "distributor.stampSheetResult.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new StampSheetResult {
                StampSheetResultId = StampSheetResultId,
                UserId = UserId,
                TransactionId = TransactionId,
                VerifyTaskRequests = VerifyTaskRequests.Clone() as Gs2.Core.Model.VerifyAction[],
                TaskRequests = TaskRequests.Clone() as Gs2.Core.Model.ConsumeAction[],
                SheetRequest = SheetRequest.Clone() as Gs2.Core.Model.AcquireAction,
                VerifyTaskResultCodes = VerifyTaskResultCodes.Clone() as int[],
                VerifyTaskResults = VerifyTaskResults.Clone() as string[],
                TaskResultCodes = TaskResultCodes.Clone() as int[],
                TaskResults = TaskResults.Clone() as string[],
                SheetResultCode = SheetResultCode,
                SheetResult = SheetResult,
                NextTransactionId = NextTransactionId,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}