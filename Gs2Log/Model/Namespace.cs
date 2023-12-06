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
	public class Namespace : IComparable
	{
        public string NamespaceId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Type { set; get; }
        public string GcpCredentialJson { set; get; }
        public string BigQueryDatasetName { set; get; }
        public int? LogExpireDays { set; get; }
        public string AwsRegion { set; get; }
        public string AwsAccessKeyId { set; get; }
        public string AwsSecretAccessKey { set; get; }
        public string FirehoseStreamName { set; get; }
        public string Status { set; get; }
        public long? CreatedAt { set; get; }
        public long? UpdatedAt { set; get; }
        public long? Revision { set; get; }
        public Namespace WithNamespaceId(string namespaceId) {
            this.NamespaceId = namespaceId;
            return this;
        }
        public Namespace WithName(string name) {
            this.Name = name;
            return this;
        }
        public Namespace WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public Namespace WithType(string type) {
            this.Type = type;
            return this;
        }
        public Namespace WithGcpCredentialJson(string gcpCredentialJson) {
            this.GcpCredentialJson = gcpCredentialJson;
            return this;
        }
        public Namespace WithBigQueryDatasetName(string bigQueryDatasetName) {
            this.BigQueryDatasetName = bigQueryDatasetName;
            return this;
        }
        public Namespace WithLogExpireDays(int? logExpireDays) {
            this.LogExpireDays = logExpireDays;
            return this;
        }
        public Namespace WithAwsRegion(string awsRegion) {
            this.AwsRegion = awsRegion;
            return this;
        }
        public Namespace WithAwsAccessKeyId(string awsAccessKeyId) {
            this.AwsAccessKeyId = awsAccessKeyId;
            return this;
        }
        public Namespace WithAwsSecretAccessKey(string awsSecretAccessKey) {
            this.AwsSecretAccessKey = awsSecretAccessKey;
            return this;
        }
        public Namespace WithFirehoseStreamName(string firehoseStreamName) {
            this.FirehoseStreamName = firehoseStreamName;
            return this;
        }
        public Namespace WithStatus(string status) {
            this.Status = status;
            return this;
        }
        public Namespace WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }
        public Namespace WithUpdatedAt(long? updatedAt) {
            this.UpdatedAt = updatedAt;
            return this;
        }
        public Namespace WithRevision(long? revision) {
            this.Revision = revision;
            return this;
        }

        private static System.Text.RegularExpressions.Regex _regionRegex = new System.Text.RegularExpressions.Regex(
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+)",
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
                @"grn:gs2:(?<region>.+):(?<ownerId>.+):log:(?<namespaceName>.+)",
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

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static Namespace FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new Namespace()
                .WithNamespaceId(!data.Keys.Contains("namespaceId") || data["namespaceId"] == null ? null : data["namespaceId"].ToString())
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithGcpCredentialJson(!data.Keys.Contains("gcpCredentialJson") || data["gcpCredentialJson"] == null ? null : data["gcpCredentialJson"].ToString())
                .WithBigQueryDatasetName(!data.Keys.Contains("bigQueryDatasetName") || data["bigQueryDatasetName"] == null ? null : data["bigQueryDatasetName"].ToString())
                .WithLogExpireDays(!data.Keys.Contains("logExpireDays") || data["logExpireDays"] == null ? null : (int?)(data["logExpireDays"].ToString().Contains(".") ? (int)double.Parse(data["logExpireDays"].ToString()) : int.Parse(data["logExpireDays"].ToString())))
                .WithAwsRegion(!data.Keys.Contains("awsRegion") || data["awsRegion"] == null ? null : data["awsRegion"].ToString())
                .WithAwsAccessKeyId(!data.Keys.Contains("awsAccessKeyId") || data["awsAccessKeyId"] == null ? null : data["awsAccessKeyId"].ToString())
                .WithAwsSecretAccessKey(!data.Keys.Contains("awsSecretAccessKey") || data["awsSecretAccessKey"] == null ? null : data["awsSecretAccessKey"].ToString())
                .WithFirehoseStreamName(!data.Keys.Contains("firehoseStreamName") || data["firehoseStreamName"] == null ? null : data["firehoseStreamName"].ToString())
                .WithStatus(!data.Keys.Contains("status") || data["status"] == null ? null : data["status"].ToString())
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())))
                .WithUpdatedAt(!data.Keys.Contains("updatedAt") || data["updatedAt"] == null ? null : (long?)(data["updatedAt"].ToString().Contains(".") ? (long)double.Parse(data["updatedAt"].ToString()) : long.Parse(data["updatedAt"].ToString())))
                .WithRevision(!data.Keys.Contains("revision") || data["revision"] == null ? null : (long?)(data["revision"].ToString().Contains(".") ? (long)double.Parse(data["revision"].ToString()) : long.Parse(data["revision"].ToString())));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["namespaceId"] = NamespaceId,
                ["name"] = Name,
                ["description"] = Description,
                ["type"] = Type,
                ["gcpCredentialJson"] = GcpCredentialJson,
                ["bigQueryDatasetName"] = BigQueryDatasetName,
                ["logExpireDays"] = LogExpireDays,
                ["awsRegion"] = AwsRegion,
                ["awsAccessKeyId"] = AwsAccessKeyId,
                ["awsSecretAccessKey"] = AwsSecretAccessKey,
                ["firehoseStreamName"] = FirehoseStreamName,
                ["status"] = Status,
                ["createdAt"] = CreatedAt,
                ["updatedAt"] = UpdatedAt,
                ["revision"] = Revision,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceId != null) {
                writer.WritePropertyName("namespaceId");
                writer.Write(NamespaceId.ToString());
            }
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Type != null) {
                writer.WritePropertyName("type");
                writer.Write(Type.ToString());
            }
            if (GcpCredentialJson != null) {
                writer.WritePropertyName("gcpCredentialJson");
                writer.Write(GcpCredentialJson.ToString());
            }
            if (BigQueryDatasetName != null) {
                writer.WritePropertyName("bigQueryDatasetName");
                writer.Write(BigQueryDatasetName.ToString());
            }
            if (LogExpireDays != null) {
                writer.WritePropertyName("logExpireDays");
                writer.Write((LogExpireDays.ToString().Contains(".") ? (int)double.Parse(LogExpireDays.ToString()) : int.Parse(LogExpireDays.ToString())));
            }
            if (AwsRegion != null) {
                writer.WritePropertyName("awsRegion");
                writer.Write(AwsRegion.ToString());
            }
            if (AwsAccessKeyId != null) {
                writer.WritePropertyName("awsAccessKeyId");
                writer.Write(AwsAccessKeyId.ToString());
            }
            if (AwsSecretAccessKey != null) {
                writer.WritePropertyName("awsSecretAccessKey");
                writer.Write(AwsSecretAccessKey.ToString());
            }
            if (FirehoseStreamName != null) {
                writer.WritePropertyName("firehoseStreamName");
                writer.Write(FirehoseStreamName.ToString());
            }
            if (Status != null) {
                writer.WritePropertyName("status");
                writer.Write(Status.ToString());
            }
            if (CreatedAt != null) {
                writer.WritePropertyName("createdAt");
                writer.Write((CreatedAt.ToString().Contains(".") ? (long)double.Parse(CreatedAt.ToString()) : long.Parse(CreatedAt.ToString())));
            }
            if (UpdatedAt != null) {
                writer.WritePropertyName("updatedAt");
                writer.Write((UpdatedAt.ToString().Contains(".") ? (long)double.Parse(UpdatedAt.ToString()) : long.Parse(UpdatedAt.ToString())));
            }
            if (Revision != null) {
                writer.WritePropertyName("revision");
                writer.Write((Revision.ToString().Contains(".") ? (long)double.Parse(Revision.ToString()) : long.Parse(Revision.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as Namespace;
            var diff = 0;
            if (NamespaceId == null && NamespaceId == other.NamespaceId)
            {
                // null and null
            }
            else
            {
                diff += NamespaceId.CompareTo(other.NamespaceId);
            }
            if (Name == null && Name == other.Name)
            {
                // null and null
            }
            else
            {
                diff += Name.CompareTo(other.Name);
            }
            if (Description == null && Description == other.Description)
            {
                // null and null
            }
            else
            {
                diff += Description.CompareTo(other.Description);
            }
            if (Type == null && Type == other.Type)
            {
                // null and null
            }
            else
            {
                diff += Type.CompareTo(other.Type);
            }
            if (GcpCredentialJson == null && GcpCredentialJson == other.GcpCredentialJson)
            {
                // null and null
            }
            else
            {
                diff += GcpCredentialJson.CompareTo(other.GcpCredentialJson);
            }
            if (BigQueryDatasetName == null && BigQueryDatasetName == other.BigQueryDatasetName)
            {
                // null and null
            }
            else
            {
                diff += BigQueryDatasetName.CompareTo(other.BigQueryDatasetName);
            }
            if (LogExpireDays == null && LogExpireDays == other.LogExpireDays)
            {
                // null and null
            }
            else
            {
                diff += (int)(LogExpireDays - other.LogExpireDays);
            }
            if (AwsRegion == null && AwsRegion == other.AwsRegion)
            {
                // null and null
            }
            else
            {
                diff += AwsRegion.CompareTo(other.AwsRegion);
            }
            if (AwsAccessKeyId == null && AwsAccessKeyId == other.AwsAccessKeyId)
            {
                // null and null
            }
            else
            {
                diff += AwsAccessKeyId.CompareTo(other.AwsAccessKeyId);
            }
            if (AwsSecretAccessKey == null && AwsSecretAccessKey == other.AwsSecretAccessKey)
            {
                // null and null
            }
            else
            {
                diff += AwsSecretAccessKey.CompareTo(other.AwsSecretAccessKey);
            }
            if (FirehoseStreamName == null && FirehoseStreamName == other.FirehoseStreamName)
            {
                // null and null
            }
            else
            {
                diff += FirehoseStreamName.CompareTo(other.FirehoseStreamName);
            }
            if (Status == null && Status == other.Status)
            {
                // null and null
            }
            else
            {
                diff += Status.CompareTo(other.Status);
            }
            if (CreatedAt == null && CreatedAt == other.CreatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(CreatedAt - other.CreatedAt);
            }
            if (UpdatedAt == null && UpdatedAt == other.UpdatedAt)
            {
                // null and null
            }
            else
            {
                diff += (int)(UpdatedAt - other.UpdatedAt);
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