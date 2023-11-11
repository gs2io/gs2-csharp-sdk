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
using Gs2.Gs2Experience.Model;
using Gs2.Util.LitJson;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Experience.Request
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	[System.Serializable]
	public class MultiplyAcquireActionsByUserIdRequest : Gs2Request<MultiplyAcquireActionsByUserIdRequest>
	{
        public string NamespaceName { set; get; }
        public string UserId { set; get; }
        public string ExperienceName { set; get; }
        public string PropertyId { set; get; }
        public string RateName { set; get; }
        public Gs2.Core.Model.AcquireAction[] AcquireActions { set; get; }
        public string DuplicationAvoider { set; get; }

        public MultiplyAcquireActionsByUserIdRequest WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithExperienceName(string experienceName) {
            this.ExperienceName = experienceName;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithPropertyId(string propertyId) {
            this.PropertyId = propertyId;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithRateName(string rateName) {
            this.RateName = rateName;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithAcquireActions(Gs2.Core.Model.AcquireAction[] acquireActions) {
            this.AcquireActions = acquireActions;
            return this;
        }

        public MultiplyAcquireActionsByUserIdRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.DuplicationAvoider = duplicationAvoider;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static MultiplyAcquireActionsByUserIdRequest FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new MultiplyAcquireActionsByUserIdRequest()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithExperienceName(!data.Keys.Contains("experienceName") || data["experienceName"] == null ? null : data["experienceName"].ToString())
                .WithPropertyId(!data.Keys.Contains("propertyId") || data["propertyId"] == null ? null : data["propertyId"].ToString())
                .WithRateName(!data.Keys.Contains("rateName") || data["rateName"] == null ? null : data["rateName"].ToString())
                .WithAcquireActions(!data.Keys.Contains("acquireActions") || data["acquireActions"] == null || !data["acquireActions"].IsArray ? new Gs2.Core.Model.AcquireAction[]{} : data["acquireActions"].Cast<JsonData>().Select(v => {
                    return Gs2.Core.Model.AcquireAction.FromJson(v);
                }).ToArray());
        }

        public override JsonData ToJson()
        {
            JsonData acquireActionsJsonData = null;
            if (AcquireActions != null)
            {
                acquireActionsJsonData = new JsonData();
                foreach (var acquireAction in AcquireActions)
                {
                    acquireActionsJsonData.Add(acquireAction.ToJson());
                }
            }
            return new JsonData {
                ["namespaceName"] = NamespaceName,
                ["userId"] = UserId,
                ["experienceName"] = ExperienceName,
                ["propertyId"] = PropertyId,
                ["rateName"] = RateName,
                ["acquireActions"] = acquireActionsJsonData,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (NamespaceName != null) {
                writer.WritePropertyName("namespaceName");
                writer.Write(NamespaceName.ToString());
            }
            if (UserId != null) {
                writer.WritePropertyName("userId");
                writer.Write(UserId.ToString());
            }
            if (ExperienceName != null) {
                writer.WritePropertyName("experienceName");
                writer.Write(ExperienceName.ToString());
            }
            if (PropertyId != null) {
                writer.WritePropertyName("propertyId");
                writer.Write(PropertyId.ToString());
            }
            if (RateName != null) {
                writer.WritePropertyName("rateName");
                writer.Write(RateName.ToString());
            }
            writer.WriteArrayStart();
            foreach (var acquireAction in AcquireActions)
            {
                if (acquireAction != null) {
                    acquireAction.WriteJson(writer);
                }
            }
            writer.WriteArrayEnd();
            writer.WriteObjectEnd();
        }

        public override string UniqueKey() {
            var key = "";
            key += NamespaceName + ":";
            key += UserId + ":";
            key += ExperienceName + ":";
            key += PropertyId + ":";
            key += RateName + ":";
            key += AcquireActions + ":";
            return key;
        }

        protected override Gs2Request DoMultiple(int x) {
            return new MultiplyAcquireActionsByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                ExperienceName = ExperienceName,
                PropertyId = PropertyId,
                RateName = RateName,
                AcquireActions = AcquireActions,
            };
        }

        protected override Gs2Request DoAdd(Gs2Request x) {
            var y = (MultiplyAcquireActionsByUserIdRequest)x;
            if (NamespaceName != y.NamespaceName) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::namespaceName");
            }
            if (UserId != y.UserId) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::userId");
            }
            if (ExperienceName != y.ExperienceName) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::experienceName");
            }
            if (PropertyId != y.PropertyId) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::propertyId");
            }
            if (RateName != y.RateName) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::rateName");
            }
            if (AcquireActions != y.AcquireActions) {
                throw new ArithmeticException("mismatch parameter values MultiplyAcquireActionsByUserIdRequest::acquireActions");
            }
            return new MultiplyAcquireActionsByUserIdRequest {
                NamespaceName = NamespaceName,
                UserId = UserId,
                ExperienceName = ExperienceName,
                PropertyId = PropertyId,
                RateName = RateName,
                AcquireActions = AcquireActions,
            };
        }
    }
}