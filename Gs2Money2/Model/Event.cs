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

#pragma warning disable CS0618 // Obsolete with a message

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
	public partial class Event : IComparable
	{
        public string EventId { set; get; }
        public string TransactionId { set; get; }
        public string UserId { set; get; }
        public string EventType { set; get; }
        public Gs2.Gs2Money2.Model.VerifyReceiptEvent VerifyReceiptEvent { set; get; }
        public Gs2.Gs2Money2.Model.DepositEvent DepositEvent { set; get; }
        public Gs2.Gs2Money2.Model.WithdrawEvent WithdrawEvent { set; get; }
        public Gs2.Gs2Money2.Model.RefundEvent RefundEvent { set; get; }
        public long? CreatedAt { set; get; }
        public long? Revision { set; get; }
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
        public Event WithRefundEvent(Gs2.Gs2Money2.Model.RefundEvent refundEvent) {
            this.RefundEvent = refundEvent;
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
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new Event()
                .WithEventId(!data.Keys.Contains("eventId") || data["eventId"] == null ? null : data["eventId"].ToString())
                .WithTransactionId(!data.Keys.Contains("transactionId") || data["transactionId"] == null ? null : data["transactionId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithEventType(!data.Keys.Contains("eventType") || data["eventType"] == null ? null : data["eventType"].ToString())
                .WithVerifyReceiptEvent(!data.Keys.Contains("verifyReceiptEvent") || data["verifyReceiptEvent"] == null ? null : Gs2.Gs2Money2.Model.VerifyReceiptEvent.FromJson(data["verifyReceiptEvent"]))
                .WithDepositEvent(!data.Keys.Contains("depositEvent") || data["depositEvent"] == null ? null : Gs2.Gs2Money2.Model.DepositEvent.FromJson(data["depositEvent"]))
                .WithWithdrawEvent(!data.Keys.Contains("withdrawEvent") || data["withdrawEvent"] == null ? null : Gs2.Gs2Money2.Model.WithdrawEvent.FromJson(data["withdrawEvent"]))
                .WithRefundEvent(!data.Keys.Contains("refundEvent") || data["refundEvent"] == null ? null : Gs2.Gs2Money2.Model.RefundEvent.FromJson(data["refundEvent"]))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["createdAt"].ToString()))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : Gs2.Core.Util.JsonValue.ToNullableLong(data["revision"].ToString()));
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
                ["refundEvent"] = RefundEvent?.ToJson(),
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
            if (RefundEvent != null) {
                writer.WritePropertyName("refundEvent");
                RefundEvent.WriteJson(writer);
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
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type Event.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(EventId, other.EventId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(TransactionId, other.TransactionId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(UserId, other.UserId);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(EventType, other.EventType);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(VerifyReceiptEvent, other.VerifyReceiptEvent);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(DepositEvent, other.DepositEvent);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(WithdrawEvent, other.WithdrawEvent);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(RefundEvent, other.RefundEvent);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(CreatedAt, other.CreatedAt);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Revision, other.Revision);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
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
                    case "Refund":
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
                VerifyReceiptEvent = VerifyReceiptEvent?.Clone() as Gs2.Gs2Money2.Model.VerifyReceiptEvent,
                DepositEvent = DepositEvent?.Clone() as Gs2.Gs2Money2.Model.DepositEvent,
                WithdrawEvent = WithdrawEvent?.Clone() as Gs2.Gs2Money2.Model.WithdrawEvent,
                RefundEvent = RefundEvent?.Clone() as Gs2.Gs2Money2.Model.RefundEvent,
                CreatedAt = CreatedAt,
                Revision = Revision,
            };
        }
    }
}