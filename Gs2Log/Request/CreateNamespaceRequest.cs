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
using Gs2.Gs2Log.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Log.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Type { set; get; } = null!;
         public string GcpCredentialJson { set; get; } = null!;
         public string BigQueryDatasetName { set; get; } = null!;
         public int? LogExpireDays { set; get; } = null!;
         public string AwsRegion { set; get; } = null!;
         public string AwsAccessKeyId { set; get; } = null!;
         public string AwsSecretAccessKey { set; get; } = null!;
         public string FirehoseStreamName { set; get; } = null!;
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithType(string type) {
            this.Type = type;
            return this;
        }
        public CreateNamespaceRequest WithGcpCredentialJson(string gcpCredentialJson) {
            this.GcpCredentialJson = gcpCredentialJson;
            return this;
        }
        public CreateNamespaceRequest WithBigQueryDatasetName(string bigQueryDatasetName) {
            this.BigQueryDatasetName = bigQueryDatasetName;
            return this;
        }
        public CreateNamespaceRequest WithLogExpireDays(int? logExpireDays) {
            this.LogExpireDays = logExpireDays;
            return this;
        }
        public CreateNamespaceRequest WithAwsRegion(string awsRegion) {
            this.AwsRegion = awsRegion;
            return this;
        }
        public CreateNamespaceRequest WithAwsAccessKeyId(string awsAccessKeyId) {
            this.AwsAccessKeyId = awsAccessKeyId;
            return this;
        }
        public CreateNamespaceRequest WithAwsSecretAccessKey(string awsSecretAccessKey) {
            this.AwsSecretAccessKey = awsSecretAccessKey;
            return this;
        }
        public CreateNamespaceRequest WithFirehoseStreamName(string firehoseStreamName) {
            this.FirehoseStreamName = firehoseStreamName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CreateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CreateNamespaceRequest()
                .WithName(!data.Keys.Contains("name") || data["name"] == null ? null : data["name"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithType(!data.Keys.Contains("type") || data["type"] == null ? null : data["type"].ToString())
                .WithGcpCredentialJson(!data.Keys.Contains("gcpCredentialJson") || data["gcpCredentialJson"] == null ? null : data["gcpCredentialJson"].ToString())
                .WithBigQueryDatasetName(!data.Keys.Contains("bigQueryDatasetName") || data["bigQueryDatasetName"] == null ? null : data["bigQueryDatasetName"].ToString())
                .WithLogExpireDays(!data.Keys.Contains("logExpireDays") || data["logExpireDays"] == null ? null : (int?)(data["logExpireDays"].ToString().Contains(".") ? (int)double.Parse(data["logExpireDays"].ToString()) : int.Parse(data["logExpireDays"].ToString())))
                .WithAwsRegion(!data.Keys.Contains("awsRegion") || data["awsRegion"] == null ? null : data["awsRegion"].ToString())
                .WithAwsAccessKeyId(!data.Keys.Contains("awsAccessKeyId") || data["awsAccessKeyId"] == null ? null : data["awsAccessKeyId"].ToString())
                .WithAwsSecretAccessKey(!data.Keys.Contains("awsSecretAccessKey") || data["awsSecretAccessKey"] == null ? null : data["awsSecretAccessKey"].ToString())
                .WithFirehoseStreamName(!data.Keys.Contains("firehoseStreamName") || data["firehoseStreamName"] == null ? null : data["firehoseStreamName"].ToString());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
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
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
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
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Description + ":";
            key += Type + ":";
            key += GcpCredentialJson + ":";
            key += BigQueryDatasetName + ":";
            key += LogExpireDays + ":";
            key += AwsRegion + ":";
            key += AwsAccessKeyId + ":";
            key += AwsSecretAccessKey + ":";
            key += FirehoseStreamName + ":";
            return key;
        }
    }
}