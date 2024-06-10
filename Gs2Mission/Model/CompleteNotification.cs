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

namespace Gs2.Gs2Mission.Model
{
	public class CompleteNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string GroupName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public string TaskName { set; get; } = null!;
        public CompleteNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public CompleteNotification WithGroupName(string groupName) {
            this.GroupName = groupName;
            return this;
        }
        public CompleteNotification WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public CompleteNotification WithTaskName(string taskName) {
            this.TaskName = taskName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static CompleteNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new CompleteNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGroupName(!data.Keys.Contains("groupName") || data["groupName"] == null ? null : data["groupName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithTaskName(!data.Keys.Contains("taskName") || data["taskName"] == null ? null : data["taskName"].ToString());
        }
    }
}
