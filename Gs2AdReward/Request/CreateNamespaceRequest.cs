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
         public string Name { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.AdMob Admob { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.UnityAd UnityAd { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.AppLovinMax[] AppLovinMaxes { set; get; } = null!;
         public string Description { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.ScriptSetting AcquirePointScript { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.ScriptSetting ConsumePointScript { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.NotificationSetting ChangePointNotification { set; get; } = null!;
         public Gs2.Gs2AdReward.Model.LogSetting LogSetting { set; get; } = null!;
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
        public CreateNamespaceRequest WithAppLovinMaxes(Gs2.Gs2AdReward.Model.AppLovinMax[] appLovinMaxes) {
            this.AppLovinMaxes = appLovinMaxes;
            return this;
        }
        public CreateNamespaceRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public CreateNamespaceRequest WithAcquirePointScript(Gs2.Gs2AdReward.Model.ScriptSetting acquirePointScript) {
            this.AcquirePointScript = acquirePointScript;
            return this;
        }
        public CreateNamespaceRequest WithConsumePointScript(Gs2.Gs2AdReward.Model.ScriptSetting consumePointScript) {
            this.ConsumePointScript = consumePointScript;
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
                .WithAppLovinMaxes(!data.Keys.Contains("appLovinMaxes") || data["appLovinMaxes"] == null || !data["appLovinMaxes"].IsArray ? null : data["appLovinMaxes"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2AdReward.Model.AppLovinMax.FromJson(v);
                }).ToArray())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithAcquirePointScript(!data.Keys.Contains("acquirePointScript") || data["acquirePointScript"] == null ? null : Gs2.Gs2AdReward.Model.ScriptSetting.FromJson(data["acquirePointScript"]))
                .WithConsumePointScript(!data.Keys.Contains("consumePointScript") || data["consumePointScript"] == null ? null : Gs2.Gs2AdReward.Model.ScriptSetting.FromJson(data["consumePointScript"]))
                .WithChangePointNotification(!data.Keys.Contains("changePointNotification") || data["changePointNotification"] == null ? null : Gs2.Gs2AdReward.Model.NotificationSetting.FromJson(data["changePointNotification"]))
                .WithLogSetting(!data.Keys.Contains("logSetting") || data["logSetting"] == null ? null : Gs2.Gs2AdReward.Model.LogSetting.FromJson(data["logSetting"]));
        }

        public override JsonData ToJson()
        {
            JsonData appLovinMaxesJsonData = null;
            if (AppLovinMaxes != null && AppLovinMaxes.Length > 0)
            {
                appLovinMaxesJsonData = new JsonData();
                foreach (var appLovinMax in AppLovinMaxes)
                {
                    appLovinMaxesJsonData.Add(appLovinMax.ToJson());
                }
            }
            return new JsonData {
                ["name"] = Name,
                ["admob"] = Admob?.ToJson(),
                ["unityAd"] = UnityAd?.ToJson(),
                ["appLovinMaxes"] = appLovinMaxesJsonData,
                ["description"] = Description,
                ["acquirePointScript"] = AcquirePointScript?.ToJson(),
                ["consumePointScript"] = ConsumePointScript?.ToJson(),
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
            if (AppLovinMaxes != null) {
                writer.WritePropertyName("appLovinMaxes");
                writer.WriteArrayStart();
                foreach (var appLovinMax in AppLovinMaxes)
                {
                    if (appLovinMax != null) {
                        appLovinMax.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (AcquirePointScript != null) {
                AcquirePointScript.WriteJson(writer);
            }
            if (ConsumePointScript != null) {
                ConsumePointScript.WriteJson(writer);
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
            key += AppLovinMaxes + ":";
            key += Description + ":";
            key += AcquirePointScript + ":";
            key += ConsumePointScript + ":";
            key += ChangePointNotification + ":";
            key += LogSetting + ":";
            return key;
        }
    }
}