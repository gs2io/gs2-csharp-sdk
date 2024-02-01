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
	public class UpdateNamespaceRequest : Gs2Request<UpdateNamespaceRequest>
	{
         public string NamespaceName { set; get; }
         public string Description { set; get; }
         public Gs2.Gs2AdReward.Model.AdMob Admob { set; get; }
         public Gs2.Gs2AdReward.Model.UnityAd UnityAd { set; get; }
         public Gs2.Gs2AdReward.Model.NotificationSetting ChangePointNotification { set; get; }
         public Gs2.Gs2AdReward.Model.LogSetting LogSetting { set; get; }
        public UpdateNamespaceRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateNamespaceRequest WithAdmob(Gs2.Gs2AdReward.Model.AdMob admob) {
            this.Admob = admob;
            return this;
        }
        public UpdateNamespaceRequest WithUnityAd(Gs2.Gs2AdReward.Model.UnityAd unityAd) {
            this.UnityAd = unityAd;
            return this;
        }
        public UpdateNamespaceRequest WithChangePointNotification(Gs2.Gs2AdReward.Model.NotificationSetting changePointNotification) {
            this.ChangePointNotification = changePointNotification;
            return this;
        }
        public UpdateNamespaceRequest WithLogSetting(Gs2.Gs2AdReward.Model.LogSetting logSetting) {
            this.LogSetting = logSetting;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateNamespaceRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateNamespaceRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAdmob(!data.Keys.Contains("admob") || data["admob"] == null ? null : Gs2.Gs2AdReward.Model.AdMob.FromJson(data["admob"]))
                .WithUnityAd(!data.Keys.Contains("unityAd") || data["unityAd"] == null ? null : Gs2.Gs2AdReward.Model.UnityAd.FromJson(data["unityAd"]))
                .WithChangePointNotification(!data.Keys.Contains("changePointNotification") || data["changePointNotification"] == null ? null : Gs2.Gs2AdReward.Model.NotificationSetting.FromJson(data["changePointNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2AdReward.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["description"] = Description,
                ["admob"] = Admob?.ToJson(),
                ["unityAd"] = UnityAd?.ToJson(),
                ["changePointNotification"] = ChangePointNotification?.ToJson(),
                ["logSetting"] = LogSetting?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Admob != null) {
                Admob.WriteJson(writer);
            }
            if (UnityAd != null) {
                UnityAd.WriteJson(writer);
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
            key += NamespaceName + ":";
            key += Description + ":";
            key += Admob + ":";
            key += UnityAd + ":";
            key += ChangePointNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}