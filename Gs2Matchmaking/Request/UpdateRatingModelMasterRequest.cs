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
using Gs2.Gs2Matchmaking.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateRatingModelMasterRequest : Gs2Request<UpdateRatingModelMasterRequest>
	{
         public string NamespaceName { set; get; } = null!;
         public string RatingName { set; get; } = null!;
         public string Description { set; get; } = null!;
         public string Metadata { set; get; } = null!;
         public int? InitialValue { set; get; } = null!;
         public int? Volatility { set; get; } = null!;
        public UpdateRatingModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRatingModelMasterRequest WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }
        public UpdateRatingModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateRatingModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRatingModelMasterRequest WithInitialValue(int? initialValue) {
            this.InitialValue = initialValue;
            return this;
        }
        public UpdateRatingModelMasterRequest WithVolatility(int? volatility) {
            this.Volatility = volatility;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRatingModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRatingModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithInitialValue(!data.Keys.Contains("initialValue") || data["initialValue"] == null ? null : (int?)(data["initialValue"].ToString().Contains(".") ? (int)double.Parse(data["initialValue"].ToString()) : int.Parse(data["initialValue"].ToString())))
                .WithVolatility(!data.Keys.Contains("volatility") || data["volatility"] == null ? null : (int?)(data["volatility"].ToString().Contains(".") ? (int)double.Parse(data["volatility"].ToString()) : int.Parse(data["volatility"].ToString())));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["ratingName"] = RatingName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["initialValue"] = InitialValue,
                ["volatility"] = Volatility,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RatingName != null) {
                writer.WritePropertyName("ratingName");
                writer.Write(RatingName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (InitialValue != null) {
                writer.WritePropertyName("initialValue");
                writer.Write((InitialValue.ToString().Contains(".") ? (int)double.Parse(InitialValue.ToString()) : int.Parse(InitialValue.ToString())));
            }
            if (Volatility != null) {
                writer.WritePropertyName("volatility");
                writer.Write((Volatility.ToString().Contains(".") ? (int)double.Parse(Volatility.ToString()) : int.Parse(Volatility.ToString())));
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RatingName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += InitialValue + ":";
            key += Volatility + ":";
            return key;
        }
    }
}