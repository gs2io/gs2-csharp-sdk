/**
 * Copyright 2016-2021 Game Server Services Inc. All rights reserved.
 *
 * These coded instructions, statements, and computer programs contain
 * proprietary information of Game Server Services Inc. and are protected by Federal copyright law.
 * They may not be disclosed to third parties or copied or duplicated in any form,
 * in whole or in part, without the prior written consent of Game Server Services Inc.
*/

using System;
using System.Collections.Generic;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Model
{
	public class ChangeRatingNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string RatingName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public float? RateValue { set; get; } = null!;
        public ChangeRatingNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public ChangeRatingNotification WithRatingName(string ratingName) {
            this.RatingName = ratingName;
            return this;
        }
        public ChangeRatingNotification WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public ChangeRatingNotification WithRateValue(float? rateValue) {
            this.RateValue = rateValue;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static ChangeRatingNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new ChangeRatingNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRatingName(!data.Keys.Contains("ratingName") || data["ratingName"] == null ? null : data["ratingName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithRateValue(!data.Keys.Contains("rateValue") || data["rateValue"] == null ? null : (float?)float.Parse(data["rateValue"].ToString()));
        }
    }
}
