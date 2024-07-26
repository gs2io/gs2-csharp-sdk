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

namespace Gs2.Gs2Friend.Model
{
	public class AcceptRequestNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TargetUserId { set; get; } = null!;
        public AcceptRequestNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public AcceptRequestNotification WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public AcceptRequestNotification WithTargetUserId(string targetUserId) {
            this.TargetUserId = targetUserId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static AcceptRequestNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new AcceptRequestNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTargetUserId(!data.Keys.Contains("targetUserId") || data["targetUserId"] == null ? null : data["targetUserId"].ToString());
        }
    }
}
