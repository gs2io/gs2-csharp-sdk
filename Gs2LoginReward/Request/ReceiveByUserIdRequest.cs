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
using Gs2.Gs2LoginReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2LoginReward.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class ReceiveByUserIdRequest : Gs2Request<ReceiveByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string BonusModelName { set; get; }
        public string UserId { set; get; }
        public Gs2.Gs2LoginReward.Model.Config[] Config { set; get; }
        public string DuplicationAvoider { set; get; }
        public ReceiveByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ReceiveByUserIdRequest WithBonusModelName(string bonusModelName) {
            this.BonusModelName = bonusModelName;
            return this;
        }
        public ReceiveByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ReceiveByUserIdRequest WithConfig(Gs2.Gs2LoginReward.Model.Config[] config) {
            this.Config = config;
            return this;
        }

        public ReceiveByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ReceiveByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ReceiveByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithBonusModelName(!data.Keys.Contains("bonusModelName") || data["bonusModelName"] == null ? null : data["bonusModelName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithConfig(!data.Keys.Contains("config") || data["config"] == null ? new Gs2.Gs2LoginReward.Model.Config[]{} : data["config"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2LoginReward.Model.Config.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["bonusModelName"] = BonusModelName,
                ["userId"] = UserId,
                ["config"] = Config == null ? null : new JsonData(
                        Config.Select(v => {
                            //noinspection Convert2MethodRef
                            return v.ToJson();
                        }).ToArray()
                    ),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (BonusModelName != null) {
                writer.WritePropertyName("bonusModelName");
                writer.Write(BonusModelName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            writer.WriteArrayStart();
            foreach (var confi in Config)
            {
                if (confi != null) {
                    confi.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += BonusModelName + ":";
            key += UserId + ":";
            key += Config + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply ReceiveByUserIdRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (ReceiveByUserIdRequest)x;
            return this;
        }
    }
}