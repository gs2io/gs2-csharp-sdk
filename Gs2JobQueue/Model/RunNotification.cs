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

namespace Gs2.Gs2JobQueue.Model
{
	public class RunNotification
	{
        public string NamespaceName { set; get; }
        public string JobName { set; get; }
        public RunNotification WithNamespaceName(string namespaceName) {
            this.NamespaceName = namespaceName;
            return this;
        }
        public RunNotification WithJobName(string jobName) {
            this.JobName = jobName;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static RunNotification FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new RunNotification()
                .WithNamespaceName(!data.Keys.Contains("namespaceName") || data["namespaceName"] == null ? null : data["namespaceName"].ToString())
                .WithJobName(!data.Keys.Contains("jobName") || data["jobName"] == null ? null : data["jobName"].ToString());
        }
    }
}
