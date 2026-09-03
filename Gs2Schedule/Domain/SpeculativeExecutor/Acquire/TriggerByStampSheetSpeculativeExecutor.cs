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
 * deny overwrite
 */
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantUsingDirective
// ReSharper disable CheckNamespace
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable ArrangeThisQualifier
// ReSharper disable NotAccessedField.Local

#pragma warning disable 1998

using System;
using System.Numerics;
using System.Collections;
using System.Reflection;
using Gs2.Core.SpeculativeExecutor;
using Gs2.Core.Domain;
using Gs2.Core.Model; /* diff +++ */
using Gs2.Core.Util;
/* diff --- start
using Gs2.Core.Exception;
 diff --- end */
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Schedule.Request;
using Gs2.Gs2Schedule.Model.Cache;
using Gs2.Gs2Schedule.Model.Transaction;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;
#endif

namespace Gs2.Gs2Schedule.Domain.SpeculativeExecutor
{
    public static class TriggerByUserIdSpeculativeExecutor {

        public static string Action() {
            return "Gs2Schedule:TriggerByUserId";
        }

/* diff +++ start */
        private static bool IsEventStrategy(string triggerStrategy)
        {
            return triggerStrategy == "repeatCycleEnd" ||
                   triggerStrategy == "repeatCycleNextStart" ||
                   triggerStrategy == "absoluteEnd";
        }

        private static bool TryParseEventId(
            string eventId,
            out string region,
            out string ownerId,
            out string namespaceName,
            out string eventName
        ) {
            region = null;
            ownerId = null;
            namespaceName = null;
            eventName = null;
            if (string.IsNullOrEmpty(eventId)) {
                return false;
            }
            var segments = eventId.Split(':');
            if (segments.Length != 8 ||
                segments[0] != "grn" ||
                segments[1] != "gs2" ||
                string.IsNullOrEmpty(segments[2]) ||
                string.IsNullOrEmpty(segments[3]) ||
                segments[4] != "schedule" ||
                string.IsNullOrEmpty(segments[5]) ||
                segments[6] != "event" ||
                string.IsNullOrEmpty(segments[7])) {
                return false;
            }
            region = segments[2];
            ownerId = segments[3];
            namespaceName = segments[5];
            eventName = segments[7];
            return true;
        }

        private static long CurrentTimeMillis(AccessToken accessToken)
        {
            return UnixTime.ToUnixTime(DateTime.Now) +
                   (long)(accessToken?.TimeOffset ?? 0) * 1000L;
        }

        public static long? ResolveEventExpiration(
            TriggerByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Event eventItem,
            Gs2.Gs2Schedule.Model.RepeatSchedule repeatSchedule,
            Gs2.Gs2Schedule.Model.Trigger relativeTrigger,
            long? scheduleEndAt
        ) {
            if (eventItem == null) {
                return null;
            }
            switch (request?.TriggerStrategy) {
                case "repeatCycleEnd":
                    return repeatSchedule?.CurrentRepeatEndAt;
                case "repeatCycleNextStart":
                    return repeatSchedule?.NextRepeatStartAt;
                case "absoluteEnd":
                    if (scheduleEndAt != null) {
                        return scheduleEndAt;
                    }
                    if (eventItem.ScheduleType == "absolute") {
                        return eventItem.AbsoluteEnd;
                    }
                    if (eventItem.ScheduleType == "relative" &&
                        eventItem.RelativeTriggerName == relativeTrigger?.Name) {
                        return relativeTrigger.ExpiresAt;
                    }
                    return null;
                default:
                    return null;
            }
        }

        public static Gs2.Gs2Schedule.Model.Trigger Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            TriggerByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Trigger item,
            long? eventExpirationMillis = null
        ) {
            return Transform(
                domain,
                accessToken,
                request,
                item,
                CurrentTimeMillis(accessToken),
                eventExpirationMillis
            );
        }

        public static Gs2.Gs2Schedule.Model.Trigger Transform(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            TriggerByUserIdRequest request,
            Gs2.Gs2Schedule.Model.Trigger item,
            long currentTimeMillis,
            long? eventExpirationMillis = null
        ) {
            return item.SpeculativeTriggerAt(
                request,
                currentTimeMillis,
                eventExpirationMillis
            );
        }

/* diff +++ end */
#if UNITY_2017_1_OR_NEWER
        public static Gs2Future<Func<object>> ExecuteFuture(
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            TriggerByUserIdRequest request
        ) => ExecuteAsync(domain, accessToken, request).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public static async UniTask<Func<object>> ExecuteAsync(
#else
        public static async Task<Func<object>> ExecuteAsync(
#endif
            Gs2.Core.Domain.Gs2 domain,
            AccessToken accessToken,
            TriggerByUserIdRequest request
        ) {
/* diff --- start
            var item = await domain.Schedule.Namespace(
                request.NamespaceName
            ).AccessToken(
                accessToken
            ).Trigger(
                request.TriggerName
            ).ModelAsync();
 diff --- end */
/* diff +++ start */
            var prepared = request == null ? null :
                TriggerByUserIdRequest.FromJson(request.ToJson());
            var preparedAccessToken = AccessToken.FromJson(accessToken?.ToJson());
            if (prepared?.UserId == "#{userId}") {
                prepared.UserId = preparedAccessToken?.UserId;
            }
            if (domain?.RestSession == null ||
                string.IsNullOrEmpty(preparedAccessToken?.UserId) ||
                prepared?.UserId != preparedAccessToken.UserId ||
                string.IsNullOrEmpty(prepared.NamespaceName) ||
                string.IsNullOrEmpty(prepared.TriggerName)) {
                return null;
            }
            var isTtlStrategy = prepared.TriggerStrategy == "renew" ||
                                prepared.TriggerStrategy == "extend" ||
                                prepared.TriggerStrategy == "drop";
            var isEventStrategy = IsEventStrategy(prepared.TriggerStrategy);
            if ((!isTtlStrategy && !isEventStrategy) ||
                (isTtlStrategy && prepared.Ttl == null) ||
                (isEventStrategy && string.IsNullOrEmpty(prepared.EventId))) {
                return null;
            }
            var userId = preparedAccessToken.UserId;
            var timeOffset = preparedAccessToken.TimeOffset;
            var region = domain.RestSession.Region.DisplayName();
            var ownerId = domain.RestSession.OwnerId ?? "";
            string eventNamespaceName = null;
            string eventName = null;
            if (isEventStrategy) {
                var normalizedEventId = prepared.EventId
                    .Replace("{region}", domain.RestSession.Region.DisplayName())
                    .Replace("{ownerId}", domain.RestSession.OwnerId ?? "");
                prepared.EventId = normalizedEventId;
                if (!TryParseEventId(
                        normalizedEventId,
                        out var eventRegion,
                        out var eventOwnerId,
                        out eventNamespaceName,
                        out eventName
                    ) ||
                    eventRegion != region ||
                    eventOwnerId != ownerId) {
                    return null;
                }
            }
            var expectedTriggerId = string.Join(
                ":", "grn", "gs2", region, ownerId, "schedule",
                prepared.NamespaceName, "user", userId, "trigger",
                prepared.TriggerName
            );
            var (preparedItem, found) = ((Gs2.Gs2Schedule.Model.Trigger)null)
                .GetCache(
                    domain.Cache,
                    prepared.NamespaceName,
                    userId,
                    prepared.TriggerName,
                    timeOffset
                );
            bool IsExpectedTrigger(
                Gs2.Gs2Schedule.Model.Trigger item,
                string namespaceName,
                string triggerName
            ) {
                var triggerId = string.Join(
                    ":", "grn", "gs2", region, ownerId, "schedule",
                    namespaceName, "user", userId, "trigger", triggerName
                );
                return item != null &&
                       item.TriggerId == triggerId &&
                       item.UserId == userId &&
                       item.Name == triggerName;
            }
            if (!found || (preparedItem != null && !IsExpectedTrigger(
                    preparedItem, prepared.NamespaceName, prepared.TriggerName
                ))) {
                return null;
            }
            var preparedSnapshot = preparedItem?.ToJson().ToJson();
            var currentTimeMillis = CurrentTimeMillis(preparedAccessToken);
/* diff +++ end */

/* diff --- start
            if (item == null) {
                return () => null;
 diff --- end */
/* diff +++ start */
            bool TryResolveEventExpiration(out long? expiration) {
                expiration = null;
                if (!isEventStrategy) {
                    return true;
                }
                var (eventItem, eventFound) =
                    ((Gs2.Gs2Schedule.Model.Event)null).GetCache(
                        domain.Cache,
                        eventNamespaceName,
                        userId,
                        eventName,
                        true,
                        timeOffset
                    );
                if (!eventFound || eventItem == null ||
                    eventItem.EventId != prepared.EventId ||
                    eventItem.Name != eventName) {
                    return false;
                }
                Gs2.Gs2Schedule.Model.RepeatSchedule repeatSchedule = null;
                if (prepared.TriggerStrategy == "repeatCycleEnd" ||
                    prepared.TriggerStrategy == "repeatCycleNextStart") {
                    var repeat = ((Gs2.Gs2Schedule.Model.RepeatSchedule)null)
                        .GetCache(
                            domain.Cache,
                            eventNamespaceName,
                            userId,
                            eventName,
                            true,
                            timeOffset
                        );
                    if (!repeat.Item2 || repeat.Item1 == null) {
                        return false;
                    }
                    repeatSchedule = repeat.Item1;
                }
                Gs2.Gs2Schedule.Model.Trigger relativeTrigger = null;
                if (prepared.TriggerStrategy == "absoluteEnd" &&
                    eventItem.ScheduleType == "relative") {
                    if (string.IsNullOrEmpty(eventItem.RelativeTriggerName)) {
                        return false;
                    }
                    var relative = ((Gs2.Gs2Schedule.Model.Trigger)null)
                        .GetCache(
                            domain.Cache,
                            eventNamespaceName,
                            userId,
                            eventItem.RelativeTriggerName,
                            timeOffset
                        );
                    if (!relative.Item2 || !IsExpectedTrigger(
                            relative.Item1,
                            eventNamespaceName,
                            eventItem.RelativeTriggerName
                        )) {
                        return false;
                    }
                    relativeTrigger = relative.Item1;
                }
                expiration = ResolveEventExpiration(
                    prepared,
                    eventItem,
                    repeatSchedule,
                    relativeTrigger,
                    null
                );
                return expiration != null;
/* diff +++ end */
            }
/* diff --- start
            item = item.SpeculativeExecution(request);
 diff --- end */
/* diff +++ start */
            if (!TryResolveEventExpiration(out _)) {
                return null;
            }
/* diff +++ end */

            return () =>
            {
/* diff --- start
                item.PutCache(
 diff --- end */
/* diff +++ start */
                var (live, liveFound) =
                    ((Gs2.Gs2Schedule.Model.Trigger)null).GetCache(
                        domain.Cache,
                        prepared.NamespaceName,
                        userId,
                        prepared.TriggerName,
                        timeOffset
                    );
                if (!liveFound || (live != null && !IsExpectedTrigger(
                        live, prepared.NamespaceName, prepared.TriggerName
                    )) || !TryResolveEventExpiration(out var expiration)) {
                    return null;
                }
                if (preparedItem == null) {
                    if (live != null && live.Revision != 0) {
                        return null;
                    }
                }
                else if (live == null ||
                         (live.Revision != 0 &&
                          live.ToJson().ToJson() != preparedSnapshot)) {
                    return null;
                }
                Gs2.Gs2Schedule.Model.Trigger changed;
                try {
                    changed = Transform(
                        domain,
                        preparedAccessToken,
                        prepared,
                        live,
                        currentTimeMillis,
                        expiration
                    );
                }
                catch (Exception) {
                    return null;
                }
                if (changed == null) {
                    return null;
                }
                changed.TriggerId = expectedTriggerId;
                changed.UserId = userId;
                changed.Name = prepared.TriggerName;
                changed.Revision = 0;
                changed.PutCache(
/* diff +++ end */
                    domain.Cache,
/* diff --- start
                    request.NamespaceName,
                    request.UserId,
                    request.TriggerName,
                    null
 diff --- end */
/* diff +++ start */
                    prepared.NamespaceName,
                    userId,
                    prepared.TriggerName,
                    timeOffset
/* diff +++ end */
                );
                return null;
            };
        }
    }
}
