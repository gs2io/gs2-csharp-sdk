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

namespace Gs2.Gs2Exchange.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class Await : IComparable
	{
        public string AwaitId { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string RateName { set; get; } = null!;
        public string Name { set; get; } = null!;
        public int? Count { set; get; } = null!;
        public int? SkipSeconds { set; get; } = null!;
        public Gs2.Gs2Exchange.Model.Config[] Config { set; get; } = null!;
        public long? AcquirableAt { set; get; } = null!;
        public long? ExchangedAt { set; get; } = null!;
        public long? Revision { set; get; } = null!;
        public Await WithAwaitId(string awaitId) {
            this.AwaitId = awaitId;
            return this;
        }
        public Await WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public Await WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public Await WithName(string name) {
            this.Name = name;
            return this;
        }
        public Await WithCount(int? count) {
            this.Count = count;
            return this;
        }
        public Await WithSkipSeconds(int? skipSeconds) {
            this.SkipSeconds = skipSeconds;
            return this;
        }
        public Await WithConfig(Gs2.Gs2Exchange.Model.Config[] config) {
            this.Config = config;
            return this;
        }
        public Await WithAcquirableAt(long? acquirableAt) {
            this.AcquirableAt = acquirableAt;
            return this;
        }
        public Await WithExchangedAt(long? exchangedAt) {
            this.ExchangedAt = exchangedAt;
            return this;
        }
        public Await WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):user:(?<userId>.+):await:(?<awaitName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):user:(?<userId>.+):await:(?<awaitName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):user:(?<userId>.+):await:(?<awaitName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):user:(?<userId>.+):await:(?<awaitName>.+)",
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

        private static System.Text.RegularExpressions.Regex _awaitNameRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):exchange:(?<namespaceName>.+):user:(?<userId>.+):await:(?<awaitName>.+)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
        );

        public static string GetAwaitNameFromGrn(
            string grn
        )
        {
            var match = _awaitNameRegex.Match(grn);
            if (!match.Success || !match.Groups["awaitName"].Success)
            {
                return null;
            }
            return match.Groups["awaitName"].Value;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Await FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Await()
                .WithAwaitId(!data.Keys.Contains("awaitId") || data["awaitId"] == null ? null : data["awaitId"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithCount(!data.Keys.Contains("count") || data["count"] == null ? null : (int?)(data["count"].ToString().Contains(".") ? (int)double.Parse(data["count"].ToString()) : int.Parse(data["count"].ToString())))
                .WithSkipSeconds(!data.Keys.Contains("skipSeconds") || data["skipSeconds"] == null ? null : (int?)(data["skipSeconds"].ToString().Contains(".") ? (int)double.Parse(data["skipSeconds"].ToString()) : int.Parse(data["skipSeconds"].ToString())))
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null || !data["config"].IsArray ? null : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Exchange.Model.Config.FromJson(v);
                }).ToArray())
                .WithAcquirableAt(!data.Keys.Contains("acquirableAt") || data["acquirableAt"] == null ? null : (long?)(data["acquirableAt"].ToString().Contains(".") ? (long)double.Parse(data["acquirableAt"].ToString()) : long.Parse(data["acquirableAt"].ToString())))
                .WithExchangedAt(!data.Keys.Contains("exchangedAt") || data["exchangedAt"] == null ? null : (long?)(data["exchangedAt"].ToString().Contains(".") ? (long)double.Parse(data["exchangedAt"].ToString()) : long.Parse(data["exchangedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            JsonData configJsonData = null;
            if (Config != null && Config.Length > 0)
            {
                configJsonData = new JsonData();
                foreach (var confi in Config)
                {
                    configJsonData.Add(confi.ToJson());
                }
            }
            return new JsonData {
                ["awaitId"] = AwaitId,
                ["userId"] = UserId,
                ["rateName"] = RateName,
                ["name"] = Name,
                ["count"] = Count,
                ["skipSeconds"] = SkipSeconds,
                ["config"] = configJsonData,
                ["acquirableAt"] = AcquirableAt,
                ["exchangedAt"] = ExchangedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AwaitId != null) {
                writer.WritePropertyName("awaitId");
                writer.Write(AwaitId.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Count != null) {
                writer.WritePropertyName("count");
                writer.Write((Count.ToString().Contains(".") ? (int)double.Parse(Count.ToString()) : int.Parse(Count.ToString())));
            }
            if (SkipSeconds != null) {
                writer.WritePropertyName("skipSeconds");
                writer.Write((SkipSeconds.ToString().Contains(".") ? (int)double.Parse(SkipSeconds.ToString()) : int.Parse(SkipSeconds.ToString())));
            }
            if (Config != null) {
                writer.WritePropertyName("config");
                writer.WriteArrayStart();
                foreach (var confi in Config)
                {
                    if (confi != null) {
                        confi.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AcquirableAt != null) {
                writer.WritePropertyName("acquirableAt");
                writer.Write((AcquirableAt.ToString().Contains(".") ? (long)double.Parse(AcquirableAt.ToString()) : long.Parse(AcquirableAt.ToString())));
            }
            if (ExchangedAt != null) {
                writer.WritePropertyName("exchangedAt");
                writer.Write((ExchangedAt.ToString().Contains(".") ? (long)double.Parse(ExchangedAt.ToString()) : long.Parse(ExchangedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Await;
            var diff = 0;
            if (AwaitId == null && AwaitId == other.AwaitId)
            {
                // null and null
            }
            else
            {
                diff += AwaitId.CompareTo(other.AwaitId);
            }
            if (UserId == null && UserId == other.UserId)
            {
                // null and null
            }
            else
            {
                diff += UserId.CompareTo(other.UserId);
            }
            if (RateName == null && RateName == other.RateName)
            {
                // null and null
            }
            else
            {
                diff += RateName.CompareTo(other.RateName);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Count == null && Count == other.Count)
            {
                // null and null
            }
            else
            {
                diff += (int)(Count - other.Count);
            }
            if (SkipSeconds == null && SkipSeconds == other.SkipSeconds)
            {
                // null and null
            }
            else
            {
                diff += (int)(SkipSeconds - other.SkipSeconds);
            }
            if (Config == null && Config == other.Config)
            {
                // null and null
            }
            else
            {
                diff += Config.Length - other.Config.Length;
                for (var i = 0; i < Config.Length; i++)
                {
                    diff += Config[i].CompareTo(other.Config[i]);
                }
            }
            if (AcquirableAt == null && AcquirableAt == other.AcquirableAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(AcquirableAt - other.AcquirableAt);
            }
            if (ExchangedAt == null && ExchangedAt == other.ExchangedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(ExchangedAt - other.ExchangedAt);
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
                if (AwaitId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.awaitId.error.tooLong"),
                    });
                }
            }
            {
                if (UserId.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.userId.error.tooLong"),
                    });
                }
            }
            {
                if (RateName.Length > 128) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.rateName.error.tooLong"),
                    });
                }
            }
            {
                if (Name.Length > 36) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.name.error.tooLong"),
                    });
                }
            }
            {
                if (Count < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.count.error.invalid"),
                    });
                }
                if (Count > 10000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.count.error.invalid"),
                    });
                }
            }
            {
                if (SkipSeconds < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.skipSeconds.error.invalid"),
                    });
                }
                if (SkipSeconds > 2147483646) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.skipSeconds.error.invalid"),
                    });
                }
            }
            {
                if (Config.Length > 32) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.config.error.tooMany"),
                    });
                }
            }
            {
                if (AcquirableAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.acquirableAt.error.invalid"),
                    });
                }
                if (AcquirableAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.acquirableAt.error.invalid"),
                    });
                }
            }
            {
                if (ExchangedAt < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.exchangedAt.error.invalid"),
                    });
                }
                if (ExchangedAt > 32503680000000) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.exchangedAt.error.invalid"),
                    });
                }
            }
            {
                if (Revision < 0) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.revision.error.invalid"),
                    });
                }
                if (Revision > 9223372036854775805) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("await", "exchange.await.revision.error.invalid"),
                    });
                }
            }
        }

        public object Clone() {
            return new Await {
                AwaitId = AwaitId,
                UserId = UserId,
                RateName = RateName,
                Name = Name,
                Count = Count,
                SkipSeconds = SkipSeconds,
                Config = Config.Clone() as Gs2.Gs2Exchange.Model.Config[],
                AcquirableAt = AcquirableAt,
                ExchangedAt = ExchangedAt,
                Revision = Revision,
            };
        }
    }
}