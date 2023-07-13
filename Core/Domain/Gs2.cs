using System;
using System.Collections;
using System.Threading.Tasks;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Distributor.Model;
using Gs2.Gs2JobQueue.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#else
using System.Diagnostics;
#endif

namespace Gs2.Core.Domain
{
    public class Gs2
    {
        public static int DefaultCacheMinutes = 15;
        private DateTime _lastPingAt = DateTime.Now;

        private readonly CacheDatabase _cache;
        private readonly JobQueueDomain _jobQueueDomain;
        private readonly StampSheetConfiguration _sheetConfiguration;
        private readonly Gs2RestSession _restSession;
        private readonly Gs2WebSocketSession _webSocketSession;

        public readonly Gs2Account.Domain.Gs2Account Account;
        public readonly Gs2Auth.Domain.Gs2Auth Auth;
        public readonly Gs2Chat.Domain.Gs2Chat Chat;
        public readonly Gs2Datastore.Domain.Gs2Datastore Datastore;
        public readonly Gs2Deploy.Domain.Gs2Deploy Deploy;
        public readonly Gs2Dictionary.Domain.Gs2Dictionary Dictionary;
        public readonly Gs2Distributor.Domain.Gs2Distributor Distributor;
        public readonly Gs2Enhance.Domain.Gs2Enhance Enhance;
        public readonly Gs2Exchange.Domain.Gs2Exchange Exchange;
        public readonly Gs2Experience.Domain.Gs2Experience Experience;
        public readonly Gs2Formation.Domain.Gs2Formation Formation;
        public readonly Gs2Friend.Domain.Gs2Friend Friend;
        public readonly Gs2Gateway.Domain.Gs2Gateway Gateway;
        public readonly Gs2Identifier.Domain.Gs2Identifier Identifier;
        public readonly Gs2Idle.Domain.Gs2Idle Idle;
        public readonly Gs2Inbox.Domain.Gs2Inbox Inbox;
        public readonly Gs2Inventory.Domain.Gs2Inventory Inventory;
        public readonly Gs2JobQueue.Domain.Gs2JobQueue JobQueue;
        public readonly Gs2Key.Domain.Gs2Key Key;
        public readonly Gs2Limit.Domain.Gs2Limit Limit;
        public readonly Gs2LoginReward.Domain.Gs2LoginReward LoginReward;
        public readonly Gs2Lock.Domain.Gs2Lock Lock;
        public readonly Gs2Log.Domain.Gs2Log Log;
        public readonly Gs2Lottery.Domain.Gs2Lottery Lottery;
        public readonly Gs2Matchmaking.Domain.Gs2Matchmaking Matchmaking;
        public readonly Gs2MegaField.Domain.Gs2MegaField MegaField;
        public readonly Gs2Mission.Domain.Gs2Mission Mission;
        public readonly Gs2Money.Domain.Gs2Money Money;
        public readonly Gs2News.Domain.Gs2News News;
        public readonly Gs2Quest.Domain.Gs2Quest Quest;
        public readonly Gs2Ranking.Domain.Gs2Ranking Ranking;
        public readonly Gs2Realtime.Domain.Gs2Realtime Realtime;
        public readonly Gs2Schedule.Domain.Gs2Schedule Schedule;
        public readonly Gs2Script.Domain.Gs2Script Script;
        public readonly Gs2SerialKey.Domain.Gs2SerialKey SerialKey;
        public readonly Gs2Showcase.Domain.Gs2Showcase Showcase;
        public readonly Gs2Stamina.Domain.Gs2Stamina Stamina;
        public readonly Gs2Version.Domain.Gs2Version Version;

        public Gs2(
            Gs2RestSession session,
            Gs2WebSocketSession wssession = null,
            string distributorNamespaceName = null
        )
        {
            _sheetConfiguration = StampSheetConfiguration.Builder()
                .WithNamespaceName(distributorNamespaceName)
                .Build();
            _restSession = session;
            _webSocketSession = wssession;
            _cache = new CacheDatabase();
            _jobQueueDomain = new JobQueueDomain(this);

            Account = new Gs2Account.Domain.Gs2Account(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Auth = new Gs2Auth.Domain.Gs2Auth(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Chat = new Gs2Chat.Domain.Gs2Chat(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Datastore = new Gs2Datastore.Domain.Gs2Datastore(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Deploy = new Gs2Deploy.Domain.Gs2Deploy(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Dictionary =
                new Gs2Dictionary.Domain.Gs2Dictionary(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Distributor =
                new Gs2Distributor.Domain.Gs2Distributor(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Enhance = new Gs2Enhance.Domain.Gs2Enhance(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Exchange = new Gs2Exchange.Domain.Gs2Exchange(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Experience =
                new Gs2Experience.Domain.Gs2Experience(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Formation = new Gs2Formation.Domain.Gs2Formation(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Friend = new Gs2Friend.Domain.Gs2Friend(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Gateway = new Gs2Gateway.Domain.Gs2Gateway(_cache, _jobQueueDomain, _sheetConfiguration, session,
                wssession);
            Identifier =
                new Gs2Identifier.Domain.Gs2Identifier(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Idle = new Gs2Idle.Domain.Gs2Idle(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Inbox = new Gs2Inbox.Domain.Gs2Inbox(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Inventory = new Gs2Inventory.Domain.Gs2Inventory(_cache, _jobQueueDomain, _sheetConfiguration, session);
            JobQueue = new Gs2JobQueue.Domain.Gs2JobQueue(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Key = new Gs2Key.Domain.Gs2Key(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Limit = new Gs2Limit.Domain.Gs2Limit(_cache, _jobQueueDomain, _sheetConfiguration, session);
            LoginReward = new Gs2LoginReward.Domain.Gs2LoginReward(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Lock = new Gs2Lock.Domain.Gs2Lock(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Log = new Gs2Log.Domain.Gs2Log(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Lottery = new Gs2Lottery.Domain.Gs2Lottery(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Matchmaking =
                new Gs2Matchmaking.Domain.Gs2Matchmaking(_cache, _jobQueueDomain, _sheetConfiguration, session);
            MegaField =
                new Gs2MegaField.Domain.Gs2MegaField(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Mission = new Gs2Mission.Domain.Gs2Mission(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Money = new Gs2Money.Domain.Gs2Money(_cache, _jobQueueDomain, _sheetConfiguration, session);
            News = new Gs2News.Domain.Gs2News(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Quest = new Gs2Quest.Domain.Gs2Quest(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Ranking = new Gs2Ranking.Domain.Gs2Ranking(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Realtime = new Gs2Realtime.Domain.Gs2Realtime(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Schedule = new Gs2Schedule.Domain.Gs2Schedule(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Script = new Gs2Script.Domain.Gs2Script(_cache, _jobQueueDomain, _sheetConfiguration, session);
            SerialKey = new Gs2SerialKey.Domain.Gs2SerialKey(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Showcase = new Gs2Showcase.Domain.Gs2Showcase(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Stamina = new Gs2Stamina.Domain.Gs2Stamina(_cache, _jobQueueDomain, _sheetConfiguration, session);
            Version = new Gs2Version.Domain.Gs2Version(_cache, _jobQueueDomain, _sheetConfiguration, session);

            if (wssession != null)
            {
                wssession.OnNotificationMessage += message =>
                {
                    if (message.subject.Contains(":"))
                    {
                        var service = message.subject.Substring(0, message.subject.IndexOf(':'));
                        var method = message.subject.Substring(message.subject.IndexOf(':') + 1);
                        switch (service)
                        {
                            case "Gs2Account":
                                Account.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Auth":
                                Auth.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Chat":
                                Chat.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Datastore":
                                Datastore.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Deploy":
                                Deploy.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Dictionary":
                                Dictionary.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Distributor":
                                Distributor.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Enhance":
                                Enhance.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Exchange":
                                Exchange.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Experience":
                                Experience.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Formation":
                                Formation.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Friend":
                                Friend.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Gateway":
                                Gateway.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Identifier":
                                Identifier.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Idle":
                                Idle.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Inbox":
                                Inbox.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Inventory":
                                Inventory.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2JobQueue":
                                JobQueue.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Key":
                                Key.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Limit":
                                Limit.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2LoginReward":
                                LoginReward.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Lock":
                                Lock.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Log":
                                Log.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Lottery":
                                Lottery.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Matchmaking":
                                Matchmaking.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2MegaField":
                                MegaField.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Mission":
                                Mission.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Money":
                                Money.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2News":
                                News.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Quest":
                                Quest.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Ranking":
                                Ranking.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Realtime":
                                Realtime.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Schedule":
                                Schedule.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Script":
                                Script.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2SerialKey":
                                SerialKey.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Showcase":
                                Showcase.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Stamina":
                                Stamina.HandleNotification(_cache, method, message.payload);
                                break;
                            case "Gs2Version":
                                Version.HandleNotification(_cache, method, message.payload);
                                break;
                        }
                    }
                };
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public void ClearCache<TKind>(
            string parentKey,
            string key
        ) {
            _cache.Delete<TKind>(parentKey, key);
        }
#if UNITY_2017_1_OR_NEWER
#if !GS2_ENABLE_UNITASK
        public Gs2Future<bool> Dispatch(
            AccessToken accessToken
        )
        {
            IEnumerator Impl(Gs2Future<bool> self)
            {
                while (true)
                {
                    if (DateTime.Now - _lastPingAt > TimeSpan.FromMinutes(5))
                    {
                        _webSocketSession?.Ping();
                        _lastPingAt = DateTime.Now;
                    }
                    
                    {
                        var future = Gs2Distributor.Domain.Gs2Distributor.Dispatch(
                            _cache,
                            _jobQueueDomain,
                            _sheetConfiguration,
                            _restSession,
                            accessToken
                        );
                        yield return future;
                        if (future != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    {
                        var future = Gs2JobQueue.Domain.Gs2JobQueue.Dispatch(
                            _cache,
                            _restSession,
                            accessToken
                        );
                        yield return future;
                        if (future != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    {
                        Gs2Future<bool> future = _jobQueueDomain.Run(
                            accessToken
                        );
                        yield return future;
                        if (future != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }

                        if (future.Result)
                        {
                            break;
                        }
                    }
                }
            }

            return new Gs2InlineFuture<bool>(Impl);
        }

        public Gs2Future DispatchByUserId(
            string userId
        )
        {
            IEnumerator Impl(Gs2Future self)
            {
                while (true)
                {
                    var future = _jobQueueDomain.RunByUserId(
                        userId
                    );
                    yield return future;
                    if (future != null)
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                    if (future.Result)
                    {
                        break;
                    }
                }
            }

            return new Gs2InlineFuture(Impl);
        }
#else
        public async Task DispatchAsync(
            AccessToken accessToken
        )
        {
            while (true)
            {
                if (DateTime.Now - _lastPingAt > TimeSpan.FromMinutes(5))
                {
                    _webSocketSession?.Ping();
                    _lastPingAt = DateTime.Now;
                }

                await Gs2Distributor.Domain.Gs2Distributor.Dispatch(
                    _cache,
                    _jobQueueDomain,
                    _sheetConfiguration,
                    _restSession,
                    accessToken
                );

                await Gs2JobQueue.Domain.Gs2JobQueue.Dispatch(
                    _cache,
                    _restSession,
                    accessToken
                );

                if (await _jobQueueDomain.Run(
                        accessToken
                    ))
                {
                    break;
                }
            }
        }

        public async Task DispatchByUserIdAsync(
            string userId
        )
        {
            while (true)
            {
                if (await _jobQueueDomain.RunByUserId(
                    userId
                ))
                {
                    break;
                }
            }
        }
#endif
#endif

        public static void UpdateCacheFromStampSheet(
            CacheDatabase cache,
            string action,
            string request,
            string result
        )
        {
            if (result == null)
            {
                return;
            }
            if (result.StartsWith("{\"message\":\""))
            {
                // error
#if UNITY_2017_1_OR_NEWER
                Debug.LogError(result);
#else
                Debug.WriteLine(result);
#endif
                return;
            }

            if (action.Contains(":"))
            {
                var service = action.Substring(0, action.IndexOf(':'));
                var method = action.Substring(action.IndexOf(':') + 1);
                switch (service)
                {
                    case "Gs2Account":
                        Gs2Account.Domain.Gs2Account.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Auth":
                        Gs2Auth.Domain.Gs2Auth.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Chat":
                        Gs2Chat.Domain.Gs2Chat.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Datastore":
                        Gs2Datastore.Domain.Gs2Datastore.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Deploy":
                        Gs2Deploy.Domain.Gs2Deploy.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        Gs2Dictionary.Domain.Gs2Dictionary.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Distributor":
                        Gs2Distributor.Domain.Gs2Distributor.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Enhance":
                        Gs2Enhance.Domain.Gs2Enhance.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Exchange":
                        Gs2Exchange.Domain.Gs2Exchange.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Experience":
                        Gs2Experience.Domain.Gs2Experience.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Formation":
                        Gs2Formation.Domain.Gs2Formation.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Friend":
                        Gs2Friend.Domain.Gs2Friend.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Gateway":
                        Gs2Gateway.Domain.Gs2Gateway.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Identifier":
                        Gs2Identifier.Domain.Gs2Identifier.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Idle":
                        Gs2Idle.Domain.Gs2Idle.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Inbox":
                        Gs2Inbox.Domain.Gs2Inbox.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Inventory":
                        Gs2Inventory.Domain.Gs2Inventory.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        Gs2JobQueue.Domain.Gs2JobQueue.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Key":
                        Gs2Key.Domain.Gs2Key.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Limit":
                        Gs2Limit.Domain.Gs2Limit.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        Gs2LoginReward.Domain.Gs2LoginReward.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Lock":
                        Gs2Lock.Domain.Gs2Lock.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Log":
                        Gs2Log.Domain.Gs2Log.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Lottery":
                        Gs2Lottery.Domain.Gs2Lottery.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        Gs2Matchmaking.Domain.Gs2Matchmaking.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2MegaField":
                        Gs2MegaField.Domain.Gs2MegaField.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Mission":
                        Gs2Mission.Domain.Gs2Mission.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Money":
                        Gs2Money.Domain.Gs2Money.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2News":
                        Gs2News.Domain.Gs2News.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Quest":
                        Gs2Quest.Domain.Gs2Quest.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Ranking":
                        Gs2Ranking.Domain.Gs2Ranking.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Realtime":
                        Gs2Realtime.Domain.Gs2Realtime.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Schedule":
                        Gs2Schedule.Domain.Gs2Schedule.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Script":
                        Gs2Script.Domain.Gs2Script.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        Gs2SerialKey.Domain.Gs2SerialKey.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Showcase":
                        Gs2Showcase.Domain.Gs2Showcase.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Stamina":
                        Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                    case "Gs2Version":
                        Gs2Version.Domain.Gs2Version.UpdateCacheFromStampSheet(cache, method, request, result);
                        break;
                }
            }
        }

        public static void UpdateCacheFromStampTask(
            CacheDatabase cache,
            string action,
            string request,
            string result
        )
        {
            if (result != null && result.StartsWith("{\"message\":\""))
            {
                // error
#if UNITY_2017_1_OR_NEWER
                Debug.LogError(result);
#else
                Debug.WriteLine(result);
#endif
                return;
            }

            if (action.Contains(":"))
            {
                var service = action.Substring(0, action.IndexOf(':'));
                var method = action.Substring(action.IndexOf(':') + 1);
                switch (service)
                {
                    case "Gs2Account":
                        Gs2Account.Domain.Gs2Account.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Auth":
                        Gs2Auth.Domain.Gs2Auth.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Chat":
                        Gs2Chat.Domain.Gs2Chat.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Datastore":
                        Gs2Datastore.Domain.Gs2Datastore.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Deploy":
                        Gs2Deploy.Domain.Gs2Deploy.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        Gs2Dictionary.Domain.Gs2Dictionary.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Distributor":
                        Gs2Distributor.Domain.Gs2Distributor.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Enhance":
                        Gs2Enhance.Domain.Gs2Enhance.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Exchange":
                        Gs2Exchange.Domain.Gs2Exchange.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Experience":
                        Gs2Experience.Domain.Gs2Experience.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Formation":
                        Gs2Formation.Domain.Gs2Formation.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Friend":
                        Gs2Friend.Domain.Gs2Friend.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Gateway":
                        Gs2Gateway.Domain.Gs2Gateway.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Identifier":
                        Gs2Identifier.Domain.Gs2Identifier.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Idle":
                        Gs2Idle.Domain.Gs2Idle.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Inbox":
                        Gs2Inbox.Domain.Gs2Inbox.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Inventory":
                        Gs2Inventory.Domain.Gs2Inventory.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        Gs2JobQueue.Domain.Gs2JobQueue.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Key":
                        Gs2Key.Domain.Gs2Key.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Limit":
                        Gs2Limit.Domain.Gs2Limit.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        Gs2LoginReward.Domain.Gs2LoginReward.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Lock":
                        Gs2Lock.Domain.Gs2Lock.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Log":
                        Gs2Log.Domain.Gs2Log.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Lottery":
                        Gs2Lottery.Domain.Gs2Lottery.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        Gs2Matchmaking.Domain.Gs2Matchmaking.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2MegaField":
                        Gs2MegaField.Domain.Gs2MegaField.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Mission":
                        Gs2Mission.Domain.Gs2Mission.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Money":
                        Gs2Money.Domain.Gs2Money.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2News":
                        Gs2News.Domain.Gs2News.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Quest":
                        Gs2Quest.Domain.Gs2Quest.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Ranking":
                        Gs2Ranking.Domain.Gs2Ranking.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Realtime":
                        Gs2Realtime.Domain.Gs2Realtime.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Schedule":
                        Gs2Schedule.Domain.Gs2Schedule.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Script":
                        Gs2Script.Domain.Gs2Script.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        Gs2SerialKey.Domain.Gs2SerialKey.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Showcase":
                        Gs2Showcase.Domain.Gs2Showcase.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Stamina":
                        Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                    case "Gs2Version":
                        Gs2Version.Domain.Gs2Version.UpdateCacheFromStampTask(cache, method, request, result);
                        break;
                }
            }
        }

        public static void PushJobQueue(
            JobQueueDomain jobQueueDomain,
            string namespaceName
        )
        {
            jobQueueDomain.Push(namespaceName);
        }

        public static void UpdateCacheFromJobResult(
            CacheDatabase cache,
            Job job,
            JobResultBody result
        )
        {
            if (job.ScriptId.Split(':').Length > 4)
            {
                if (job.ScriptId.Split(':')[3] == "system")
                {
                    var scriptName = job.ScriptId.Substring(job.ScriptId.LastIndexOf(':') + 1);
                    if (scriptName.StartsWith("execute_"))
                    {
                        var scriptNameTemp = scriptName.Replace("execute_", "");
                        var service = scriptNameTemp.Split('_')[0];
                        var method = scriptNameTemp.Substring(scriptNameTemp.IndexOf("_", StringComparison.Ordinal) + 1);
                        switch (service)
                        {
                            case "account":
                                Gs2Account.Domain.Gs2Account.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "auth":
                                Gs2Auth.Domain.Gs2Auth.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "chat":
                                Gs2Chat.Domain.Gs2Chat.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "datastore":
                                Gs2Datastore.Domain.Gs2Datastore.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "deploy":
                                Gs2Deploy.Domain.Gs2Deploy.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "dictionary":
                                Gs2Dictionary.Domain.Gs2Dictionary.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "distributor":
                                Gs2Distributor.Domain.Gs2Distributor.UpdateCacheFromJobResult(cache, method, job,
                                    result);
                                break;
                            case "enhance":
                                Gs2Enhance.Domain.Gs2Enhance.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "exchange":
                                Gs2Exchange.Domain.Gs2Exchange.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "experience":
                                Gs2Experience.Domain.Gs2Experience.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "formation":
                                Gs2Formation.Domain.Gs2Formation.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "friend":
                                Gs2Friend.Domain.Gs2Friend.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "gateway":
                                Gs2Gateway.Domain.Gs2Gateway.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "identifier":
                                Gs2Identifier.Domain.Gs2Identifier.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "idle":
                                Gs2Idle.Domain.Gs2Idle.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "inbox":
                                Gs2Inbox.Domain.Gs2Inbox.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "inventory":
                                Gs2Inventory.Domain.Gs2Inventory.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "job_queue":
                                Gs2JobQueue.Domain.Gs2JobQueue.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "key":
                                Gs2Key.Domain.Gs2Key.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "limit":
                                Gs2Limit.Domain.Gs2Limit.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "login_reward":
                                Gs2LoginReward.Domain.Gs2LoginReward.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "lock":
                                Gs2Lock.Domain.Gs2Lock.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "log":
                                Gs2Log.Domain.Gs2Log.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "lottery":
                                Gs2Lottery.Domain.Gs2Lottery.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "matchmaking":
                                Gs2Matchmaking.Domain.Gs2Matchmaking.UpdateCacheFromJobResult(cache, method, job,
                                    result);
                                break;
                            case "mega_field":
                                Gs2MegaField.Domain.Gs2MegaField.UpdateCacheFromJobResult(cache, method, job,
                                    result);
                                break;
                            case "mission":
                                Gs2Mission.Domain.Gs2Mission.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "money":
                                Gs2Money.Domain.Gs2Money.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "news":
                                Gs2News.Domain.Gs2News.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "quest":
                                Gs2Quest.Domain.Gs2Quest.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "ranking":
                                Gs2Ranking.Domain.Gs2Ranking.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "realtime":
                                Gs2Realtime.Domain.Gs2Realtime.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "schedule":
                                Gs2Schedule.Domain.Gs2Schedule.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "script":
                                Gs2Script.Domain.Gs2Script.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "serial_key":
                                Gs2SerialKey.Domain.Gs2SerialKey.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "showcase":
                                Gs2Showcase.Domain.Gs2Showcase.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "stamina":
                                Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "version":
                                Gs2Version.Domain.Gs2Version.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                        }
                    }
                }
            }
        }
#if GS2_ENABLE_UNITASK
        public async UniTask Disconnect()
        {
            await _restSession.CloseAsync();
            await _webSocketSession.CloseAsync();
        }
#else
        public IEnumerator Disconnect()
        {
            yield return _restSession.Close(() => {});
            yield return _webSocketSession.Close(() => {});
        }
#endif
    }
}