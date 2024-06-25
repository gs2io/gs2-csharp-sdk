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

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Event : IComparable
	{
        public string EventId { set; get; } = null!;
        public string TransactionId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string EventType { set; get; } = null!;
        public Gs2.Gs2Money2.Model.VerifyReceiptEvent VerifyReceiptEvent { set; get; } = null!;
        public Gs2.Gs2Money2.Model.DepositEvent DepositEvent { set; get; } = null!;
        public Gs2.Gs2Money2.Model.WithdrawEvent WithdrawEvent { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Event WithEventId(string eventId) {
            this.EventId = eventId;
            return this;
        }
        public Event WithTransactionId(string transactionId) {
            this.TransactionId = transactionId;
            return this;
        }
        public Event WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Event WithEventType(string eventType) {
            this.EventType = eventType;
            return this;
        }
        public Event WithVerifyReceiptEvent(Gs2.Gs2Money2.Model.VerifyReceiptEvent verifyReceiptEvent) {
            this.VerifyReceiptEvent = verifyReceiptEvent;
            return this;
        }
        public Event WithDepositEvent(Gs2.Gs2Money2.Model.DepositEvent depositEvent) {
            this.DepositEvent = depositEvent;
            return this;
        }
        public Event WithWithdrawEvent(Gs2.Gs2Money2.Model.WithdrawEvent withdrawEvent) {
            this.WithdrawEvent = withdrawEvent;
            return this;
        }
        public Event WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Event WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):event:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):event:(?<transactionId>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):event:(?<transactionId>.+)",
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

        private static System.Text.RegularExpressions.Regex _transactionIdRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):money2:(?<namespaceName>.+):event:(?<transactionId>.+)",
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
        public static Event FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Event()
                .WithEventId(!data.Keys.Contains("eventId") || data["eventId"] == null ? null : data["eventId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithEventType(!data.Keys.Contains("eventType") || data["eventType"] == null ? null : data["eventType"].ToString())
                .WithVerifyReceiptEvent(!data.Keys.Contains("verifyReceiptEvent") || data["verifyReceiptEvent"] == null ? null : Gs2.Gs2Money2.Model.VerifyReceiptEvent.FromJson(data["verifyReceiptEvent"]))
                .WithDepositEvent(!data.Keys.Contains("depositEvent") || data["depositEvent"] == null ? null : Gs2.Gs2Money2.Model.DepositEvent.FromJson(data["depositEvent"]))
                .WithWithdrawEvent(!data.Keys.Contains("withdrawEvent") || data["withdrawEvent"] == null ? null : Gs2.Gs2Money2.Model.WithdrawEvent.FromJson(data["withdrawEvent"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["eventId"] = EventId,
                ["transactionId"] = TransactionId,
                ["userId"] = UserId,
                ["eventType"] = EventType,
                ["verifyReceiptEvent"] = VerifyReceiptEvent?.ToJson(),
                ["depositEvent"] = DepositEvent?.ToJson(),
                ["withdrawEvent"] = WithdrawEvent?.ToJson(),
                ["createdAt"] = CreatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (EventId != null) {
                writer.WritePropertyName("eventId");
                writer.Write(EventId.ToString());
            }
            if (TransactionId != null) {
                writer.WritePropertyName("transactionId");
                writer.Write(TransactionId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (EventType != null) {
                writer.WritePropertyName("eventType");
                writer.Write(EventType.ToString());
            }
            if (VerifyReceiptEvent != null) {
                writer.WritePropertyName("verifyReceiptEvent");
                VerifyReceiptEvent.WriteJson(writer);
            }
            if (DepositEvent != null) {
                writer.WritePropertyName("depositEvent");
                DepositEvent.WriteJson(writer);
            }
            if (WithdrawEvent != null) {
                writer.WritePropertyName("withdrawEvent");
                WithdrawEvent.WriteJson(writer);
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
            var other = obj as Event;
            var diff = 0;
            if (EventId == null && EventId == other.EventId)
            {
                // null and null
            }
            else
            {
                diff += EventId.CompareTo(other.EventId);
            }
            if (TransactionId == null && TransactionId == other.TransactionId)
            {
                // null and null
            }
            else
            {
                diff += TransactionId.CompareTo(other.TransactionId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (EventType == null && EventType == other.EventType)
            {
                // null and null
            }
            else
            {
                diff += EventType.CompareTo(other.EventType);
            }
            if (VerifyReceiptEvent == null && VerifyReceiptEvent == other.VerifyReceiptEvent)
            {
                // null and null
            }
            else
            {
                diff += VerifyReceiptEvent.CompareTo(other.VerifyReceiptEvent);
            }
            if (DepositEvent == null && DepositEvent == other.DepositEvent)
            {
                // null and null
            }
            else
            {
                diff += DepositEvent.CompareTo(other.DepositEvent);
            }
            if (WithdrawEvent == null && WithdrawEvent == other.WithdrawEvent)
            {
                // null and null
            }
            else
            {
                diff += WithdrawEvent.CompareTo(other.WithdrawEvent);
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
                if (EventId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.eventId.error.tooLong"),
                    });
                }
            }
            {
                if (TransactionId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.transactionId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.userId.error.tooLong"),
                    });
                }
            }
            {
                switch (EventType) {
                    case "VerifyReceipt":
                    case "Deposit":
                    case "Withdraw":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("event", "money2.event.eventType.error.invalid"),
                        });
                }
            }
            {
            }
            {
            }
            {
            }
            {
                if (CreatedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.createdAt.error.invalid"),
                    });
                }
                if (CreatedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.createdAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("event", "money2.event.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Event {
                EventId = EventId,
                TransactionId = TransactionId,
                UserId = UserId,
                EventType = EventType,
                VerifyReceiptEvent = VerifyReceiptEvent.Clone() as Gs2.Gs2Money2.Model.VerifyReceiptEvent,
                DepositEvent = DepositEvent.Clone() as Gs2.Gs2Money2.Model.DepositEvent,
                WithdrawEvent = WithdrawEvent.Clone() as Gs2.Gs2Money2.Model.WithdrawEvent,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}