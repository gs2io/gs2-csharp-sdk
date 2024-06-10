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
	public class JoinNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string GatheringName { set; get; } = null!;
        public string JoinUserId { set; get; } = null!;
        public JoinNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public JoinNotification WithGatheringName(string gatheringName) {
            this.GatheringName = gatheringName;
            return this;
        }
        public JoinNotification WithJoinUserId(string joinUserId) {
            this.JoinUserId = joinUserId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static JoinNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new JoinNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGatheringName(!data.Keys.Contains("gatheringName") || data["gatheringName"] == null ? null : data["gatheringName"].ToString())
                .WithJoinUserId(!data.Keys.Contains("joinUserId") || data["joinUserId"] == null ? null : data["joinUserId"].ToString());
        }
    }
}
