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
using Gs2.Gs2Exchange.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Exchange.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class UpdateRateModelMasterRequest : Gs2Request<UpdateRateModelMasterRequest>
	{
        public string NamespaceName { set; get; }
        public string RateName { set; get; }
        public string Description { set; get; }
        public string Metadata { set; get; }
        public string TimingType { set; get; }
        public int? LockTime { set; get; }
        public bool? EnableSkip { set; get; }
        public Gs2.Core.Model.ConsumeAction[] SkipConsumeActions { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public Gs2.Core.Model.ConsumeAction[] ConsumeActions { set; get; }
        public UpdateRateModelMasterRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public UpdateRateModelMasterRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }
        public UpdateRateModelMasterRequest WithDescription(string description) {
            this.Description = description;
            return this;
        }
        public UpdateRateModelMasterRequest WithMetadata(string metadata) {
            this.Metadata = metadata;
            return this;
        }
        public UpdateRateModelMasterRequest WithTimingType(string timingType) {
            this.TimingType = timingType;
            return this;
        }
        public UpdateRateModelMasterRequest WithLockTime(int? lockTime) {
            this.LockTime = lockTime;
            return this;
        }
        public UpdateRateModelMasterRequest WithEnableSkip(bool? enableSkip) {
            this.EnableSkip = enableSkip;
            return this;
        }
        public UpdateRateModelMasterRequest WithSkipConsumeActions(Gs2.Core.Model.ConsumeAction[] skipConsumeActions) {
            this.SkipConsumeActions = skipConsumeActions;
            return this;
        }
        public UpdateRateModelMasterRequest WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }
        public UpdateRateModelMasterRequest WithConsumeActions(Gs2.Core.Model.ConsumeAction[] consumeActions) {
            this.ConsumeActions = consumeActions;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static UpdateRateModelMasterRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new UpdateRateModelMasterRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithDescription(!data.Keys.Contains("description") || data["description"] == null ? null : data["description"].ToString())
                .WithMetadata(!data.Keys.Contains("metadata") || data["metadata"] == null ? null : data["metadata"].ToString())
                .WithTimingType(!data.Keys.Contains("timingType") || data["timingType"] == null ? null : data["timingType"].ToString())
                .WithLockTime(!data.Keys.Contains("lockTime") || data["lockTime"] == null ? null : (int?)int.Parse(data["lockTime"].ToString()))
                .WithEnableSkip(!data.Keys.Contains("enableSkip") || data["enableSkip"] == null ? null : (bool?)bool.Parse(data["enableSkip"].ToString()))
                .WithSkipConsumeActions(!data.Keys.Contains("skipConsumeActions") || data["skipConsumeActions"] == null ? new Gs2.Core.Model.ConsumeAction[]{} : data["skipConsumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray())
                .WithConsumeActions(!data.Keys.Contains("consumeActions") || data["consumeActions"] == null ? new Gs2.Core.Model.ConsumeAction[]{} : data["consumeActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.ConsumeAction.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData skipConsumeActionsJsonData = null;
            if (SkipConsumeActions != null)
            {
                skipConsumeActionsJsonData = new JsonData();
                foreach (var skipConsumeAction in SkipConsumeActions)
                {
                    skipConsumeActionsJsonData.Add(skipConsumeAction.ToJson());
                }
            }
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            JsonData consumeActionsJsonData = null;
            if (ConsumeActions != null)
            {
                consumeActionsJsonData = new JsonData();
                foreach (var consumeAction in ConsumeActions)
                {
                    consumeActionsJsonData.Add(consumeAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["rateName"] = RateName,
                ["description"] = Description,
                ["metadata"] = Metadata,
                ["timingType"] = TimingType,
                ["lockTime"] = LockTime,
                ["enableSkip"] = EnableSkip,
                ["skipConsumeActions"] = skipConsumeActionsJsonData,
                ["acquireActions"] = acquireActionsJsonData,
                ["consumeActions"] = consumeActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            if (Description != null) {
                writer.WritePropertyName("description");
                writer.Write(Description.ToString());
            }
            if (Metadata != null) {
                writer.WritePropertyName("metadata");
                writer.Write(Metadata.ToString());
            }
            if (TimingType != null) {
                writer.WritePropertyName("timingType");
                writer.Write(TimingType.ToString());
            }
            if (LockTime != null) {
                writer.WritePropertyName("lockTime");
                writer.Write(int.Parse(LockTime.ToString()));
            }
            if (EnableSkip != null) {
                writer.WritePropertyName("enableSkip");
                writer.Write(bool.Parse(EnableSkip.ToString()));
            }
            writer.WriteArrayStart();
            foreach (var skipConsumeAction in SkipConsumeActions)
            {
                if (skipConsumeAction != null) {
                    skipConsumeAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteArrayStart();
            foreach (var acquireAction in AcquireActions)
            {
                if (acquireAction != null) {
                    acquireAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteArrayStart();
            foreach (var consumeAction in ConsumeActions)
            {
                if (consumeAction != null) {
                    consumeAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += RateName + ":";
            key += Description + ":";
            key += Metadata + ":";
            key += TimingType + ":";
            key += LockTime + ":";
            key += EnableSkip + ":";
            key += SkipConsumeActions + ":";
            key += AcquireActions + ":";
            key += ConsumeActions + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            if (x != 1) {
                throw new ArithmeticException("Unsupported multiply UpdateRateModelMasterRequest");
            }
            return this;
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (UpdateRateModelMasterRequest)x;
            return this;
        }
    }
}