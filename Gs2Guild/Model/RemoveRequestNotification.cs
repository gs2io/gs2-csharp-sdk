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

namespace Gs2.Gs2Guild.Model
{
	public class RemoveRequestNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string GuildModelName { set; get; } = null!;
        public string GuildName { set; get; } = null!;
        public string FromUserId { set; get; } = null!;
        public RemoveRequestNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RemoveRequestNotification WithGuildModelName(string guildModelName) {
            this.GuildModelName = guildModelName;
            return this;
        }
        public RemoveRequestNotification WithGuildName(string guildName) {
            this.GuildName = guildName;
            return this;
        }
        public RemoveRequestNotification WithFromUserId(string fromUserId) {
            this.FromUserId = fromUserId;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RemoveRequestNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RemoveRequestNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithGuildModelName(!data.Keys.Contains("guildModelName") || data["guildModelName"] == null ? null : data["guildModelName"].ToString())
                .WithGuildName(!data.Keys.Contains("guildName") || data["guildName"] == null ? null : data["guildName"].ToString())
                .WithFromUserId(!data.Keys.Contains("fromUserId") || data["fromUserId"] == null ? null : data["fromUserId"].ToString());
        }
    }
}
