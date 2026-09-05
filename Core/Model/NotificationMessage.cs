/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0(the "License").
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

using Gs2.Util.LitJson;
using System;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Model
{
    internal sealed class NotificationPayloadException : System.Exception
    {
        internal NotificationPayloadException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }

    internal static class NotificationPayload
    {
        internal static T Parse<T>(string payload, Func<JsonData, T> parser)
            where T : class
        {
            try
            {
                var data = JsonMapper.ToObject(payload);
                if (data == null || !data.IsObject)
                {
                    throw new InvalidOperationException("Notification payload was not a JSON object.");
                }
                return parser(data) ??
                       throw new InvalidOperationException("Notification payload could not be parsed.");
            }
            catch (NotificationPayloadException)
            {
                throw;
            }
            catch (System.Exception exception)
            {
                throw new NotificationPayloadException(
                    "Failed to parse a built-in notification payload.",
                    exception
                );
            }
        }
    }

#if UNITY_2017_1_OR_NEWER
    [Preserve]
#endif
    public class NotificationMessage
    {
        public string issuer;
        
        public string subject;
                
        public string payload;

        public static NotificationMessage FromJson(JsonData data)
        {
            return new NotificationMessage
            {
                issuer = data.Keys.Contains("issuer") ? (string)data["issuer"] : null,
                subject = data.Keys.Contains("subject") ? (string)data["subject"] : null,
                payload = data.Keys.Contains("payload") ? (string)data["payload"] : null,
            };
        }
    }
}
