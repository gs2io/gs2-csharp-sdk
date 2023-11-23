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
using Gs2.Gs2AdReward.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2AdReward.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class CreateNamespaceRequest : Gs2Request<CreateNamespaceRequest>
	{
         public string Name { set; get; }
         public Gs2.Gs2AdReward.Model.AdMob Admob { set; get; }
         public Gs2.Gs2AdReward.Model.UnityAd UnityAd { set; get; }
         public string Description { set; get; }
         public Gs2.Gs2AdReward.Model.NotificationSetting ChangePointNotification { set; get; }
         public Gs2.Gs2AdReward.Model.LogSetting LogSetting { set; get; }
        public CreateNamespaceRequest WithName(string name) {
            this.Name = name;
            return this;
        }
        public CreateNamespaceRequest WithAdmob(Gs2.Gs2AdReward.Model.AdMob admob) {
            this.Admob = admob;
            return this;
        }
        public CreateNamespaceRequest WithUnityAd(Gs2.Gs2AdReward.Model.UnityAd unityAd) {
            this.UnityAd = unityAd;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithChangePointNotification(Gs2.Gs2AdReward.Model.NotificationSetting changePointNotification) {
            this.ChangePointNotification = changePointNotification;
            return this;
        }
        public CreateNamespaceRequest WithLogSetting(Gs2.Gs2AdReward.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
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
                .WithAdmob(!data.Keys.Contains("admob") || data["admob"] == null ? null : Gs2.Gs2AdReward.Model.AdMob.FromJson(data["admob"]))
                .WithUnityAd(!data.Keys.Contains("unityAd") || data["unityAd"] == null ? null : Gs2.Gs2AdReward.Model.UnityAd.FromJson(data["unityAd"]))
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithChangePointNotification(!data.Keys.Contains("changePointNotification") || data["changePointNotification"] == null ? null : Gs2.Gs2AdReward.Model.NotificationSetting.FromJson(data["changePointNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2AdReward.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["name"] = Name,
                ["admob"] = Admob?.ToJson(),
                ["unityAd"] = UnityAd?.ToJson(),
                ["description"] = Description,
                ["changePointNotification"] = ChangePointNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Name != null) {
                writer.WritePropertyName("name");
                writer.Write(Name.ToString());
            }
            if (Admob != null) {
                Admob.WriteJson(writer);
            }
            if (UnityAd != null) {
                UnityAd.WriteJson(writer);
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (ChangePointNotification != null) {
                ChangePointNotification.WriteJson(writer);
            }
            if (LogSetting != null) {
                LogSetting.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += Name + ":";
            key += Admob + ":";
            key += UnityAd + ":";
            key += Description + ":";
            key += ChangePointNotification + ":";
            key += LogSetting + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply CreateNamespaceRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (CreateNamespaceRequest)x;
            return this;
        }
    }
}