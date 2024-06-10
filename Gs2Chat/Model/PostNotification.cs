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

namespace Gs2.Gs2Chat.Model
{
	public class PostNotification
	{
        public string NamespaceName { set; get; } = null!;
        public string RoomName { set; get; } = null!;
        public string UserId { set; get; } = null!;
        public int? Category { set; get; } = null!;
        public long? CreatedAt { set; get; } = null!;
        public PostNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public PostNotification WithRoomName(string roomName) {
            this.RoomName = roomName;
            return this;
        }
        public PostNotification WithUserId(string userId) {
            this.UserId = userId;
            return this;
        }
        public PostNotification WithCategory(int? category) {
            this.Category = category;
            return this;
        }
        public PostNotification WithCreatedAt(long? createdAt) {
            this.CreatedAt = createdAt;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PostNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new PostNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithRoomName(!data.Keys.Contains("roomName") || data["roomName"] == null ? null : data["roomName"].ToString())
                .WithUserId(!data.Keys.Contains("userId") || data["userId"] == null ? null : data["userId"].ToString())
                .WithCategory(!data.Keys.Contains("category") || data["category"] == null ? null : (int?)(data["category"].ToString().Contains(".") ? (int)double.Parse(data["category"].ToString()) : int.Parse(data["category"].ToString())))
                .WithCreatedAt(!data.Keys.Contains("createdAt") || data["createdAt"] == null ? null : (long?)(data["createdAt"].ToString().Contains(".") ? (long)double.Parse(data["createdAt"].ToString()) : long.Parse(data["createdAt"].ToString())));
        }
    }
}
