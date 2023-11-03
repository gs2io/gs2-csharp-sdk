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

        internal CacheDatabase Cache => this._cache;
        internal JobQueueDomain JobQueueDomain => this._jobQueueDomain;
        internal StampSheetConfiguration StampSheetConfiguration => this._sheetConfiguration;
        internal Gs2RestSession RestSession => this._restSession;
        internal Gs2WebSocketSession WebSocketSession => this._webSocketSession;

        public readonly Gs2Account.Domain.Gs2Account Account;
        public readonly Gs2AdReward.Domain.Gs2AdReward AdReward;
        public readonly Gs2Auth.Domain.Gs2Auth Auth;
        public readonly Gs2Chat.Domain.Gs2Chat Chat;
        public readonly Gs2Datastore.Domain.Gs2Datastore Datastore;
        public readonly Gs2Deploy.Domain.Gs2Deploy Deploy;
        public readonly Gs2Dictionary.Domain.Gs2Dictionary Dictionary;
        public readonly Gs2Distributor.Domain.Gs2Distributor Distributor;
        public readonly Gs2Enchant.Domain.Gs2Enchant Enchant;
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
        public readonly Gs2SkillTree.Domain.Gs2SkillTree SkillTree;
        public readonly Gs2Stamina.Domain.Gs2Stamina Stamina;
        public readonly Gs2StateMachine.Domain.Gs2StateMachine StateMachine;
        public readonly Gs2Version.Domain.Gs2Version Version;

        public Gs2(
            Gs2RestSession session,
            Gs2WebSocketSession wssession = null,
            string distributorNamespaceName = null
        )
        {
            this._sheetConfiguration = StampSheetConfiguration.Builder()
                .WithNamespaceName(distributorNamespaceName)
                .Build();
            this._restSession = session;
            this._webSocketSession = wssession;
            this._cache = new CacheDatabase();
            this._jobQueueDomain = new JobQueueDomain(this);

            this.Account = new Gs2Account.Domain.Gs2Account(this);
            this.AdReward = new Gs2AdReward.Domain.Gs2AdReward(this);
            this.Auth = new Gs2Auth.Domain.Gs2Auth(this);
            this.Chat = new Gs2Chat.Domain.Gs2Chat(this);
            this.Datastore = new Gs2Datastore.Domain.Gs2Datastore(this);
            this.Deploy = new Gs2Deploy.Domain.Gs2Deploy(this);
            this.Dictionary = new Gs2Dictionary.Domain.Gs2Dictionary(this);
            this.Distributor = new Gs2Distributor.Domain.Gs2Distributor(this);
            this.Enchant = new Gs2Enchant.Domain.Gs2Enchant(this);
            this.Enhance = new Gs2Enhance.Domain.Gs2Enhance(this);
            this.Exchange = new Gs2Exchange.Domain.Gs2Exchange(this);
            this.Experience = new Gs2Experience.Domain.Gs2Experience(this);
            this.Formation = new Gs2Formation.Domain.Gs2Formation(this);
            this.Friend = new Gs2Friend.Domain.Gs2Friend(this);
            this.Gateway = new Gs2Gateway.Domain.Gs2Gateway(this);
            this.Identifier = new Gs2Identifier.Domain.Gs2Identifier(this);
            this.Idle = new Gs2Idle.Domain.Gs2Idle(this);
            this.Inbox = new Gs2Inbox.Domain.Gs2Inbox(this);
            this.Inventory = new Gs2Inventory.Domain.Gs2Inventory(this);
            this.JobQueue = new Gs2JobQueue.Domain.Gs2JobQueue(this);
            this.Key = new Gs2Key.Domain.Gs2Key(this);
            this.Limit = new Gs2Limit.Domain.Gs2Limit(this);
            this.LoginReward = new Gs2LoginReward.Domain.Gs2LoginReward(this);
            this.Lock = new Gs2Lock.Domain.Gs2Lock(this);
            this.Log = new Gs2Log.Domain.Gs2Log(this);
            this.Lottery = new Gs2Lottery.Domain.Gs2Lottery(this);
            this.Matchmaking = new Gs2Matchmaking.Domain.Gs2Matchmaking(this);
            this.MegaField = new Gs2MegaField.Domain.Gs2MegaField(this);
            this.Mission = new Gs2Mission.Domain.Gs2Mission(this);
            this.Money = new Gs2Money.Domain.Gs2Money(this);
            this.News = new Gs2News.Domain.Gs2News(this);
            this.Quest = new Gs2Quest.Domain.Gs2Quest(this);
            this.Ranking = new Gs2Ranking.Domain.Gs2Ranking(this);
            this.Realtime = new Gs2Realtime.Domain.Gs2Realtime(this);
            this.Schedule = new Gs2Schedule.Domain.Gs2Schedule(this);
            this.Script = new Gs2Script.Domain.Gs2Script(this);
            this.SerialKey = new Gs2SerialKey.Domain.Gs2SerialKey(this);
            this.Showcase = new Gs2Showcase.Domain.Gs2Showcase(this);
            this.SkillTree = new Gs2SkillTree.Domain.Gs2SkillTree(this);
            this.Stamina = new Gs2Stamina.Domain.Gs2Stamina(this);
            this.StateMachine = new Gs2StateMachine.Domain.Gs2StateMachine(this);
            this.Version = new Gs2Version.Domain.Gs2Version(this);

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
                                this.Account.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2AdReward":
                                this.AdReward.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Auth":
                                this.Auth.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Chat":
                                this.Chat.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Datastore":
                                this.Datastore.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Deploy":
                                this.Deploy.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Dictionary":
                                this.Dictionary.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Distributor":
                                this.Distributor.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Enchant":
                                this.Enchant.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Enhance":
                                this.Enhance.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Exchange":
                                this.Exchange.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Experience":
                                this.Experience.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Formation":
                                this.Formation.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Friend":
                                this.Friend.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Gateway":
                                this.Gateway.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Identifier":
                                this.Identifier.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Idle":
                                this.Idle.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Inbox":
                                this.Inbox.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Inventory":
                                this.Inventory.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2JobQueue":
                                this.JobQueue.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Key":
                                this.Key.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Limit":
                                this.Limit.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2LoginReward":
                                this.LoginReward.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Lock":
                                this.Lock.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Log":
                                this.Log.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Lottery":
                                this.Lottery.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Matchmaking":
                                this.Matchmaking.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2MegaField":
                                this.MegaField.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Mission":
                                this.Mission.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Money":
                                this.Money.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2News":
                                this.News.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Quest":
                                this.Quest.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Ranking":
                                this.Ranking.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Realtime":
                                this.Realtime.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Schedule":
                                this.Schedule.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Script":
                                this.Script.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2SerialKey":
                                this.SerialKey.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Showcase":
                                this.Showcase.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2SkillTree":
                                this.SkillTree.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Stamina":
                                this.Stamina.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2StateMachine":
                                this.StateMachine.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Version":
                                this.Version.HandleNotification(this._cache, method, message.payload);
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
                            this,
                            accessToken
                        );
                        yield return future;
                        if (future.Error != null)
                        {
                            self.OnError(future.Error);
                            yield break;
                        }
                    }
                    {
                        var future = Gs2JobQueue.Domain.Gs2JobQueue.Dispatch(
                            this,
                            accessToken
                        );
                        yield return future;
                        if (future.Error != null)
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
                        if (future.Error != null)
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
                    this,
                    accessToken
                );

                await Gs2JobQueue.Domain.Gs2JobQueue.Dispatch(
                    this,
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
            string transactionId,
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
                        Gs2Account.Domain.Gs2Account.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2AdReward":
                        Gs2AdReward.Domain.Gs2AdReward.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Auth":
                        Gs2Auth.Domain.Gs2Auth.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Chat":
                        Gs2Chat.Domain.Gs2Chat.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Datastore":
                        Gs2Datastore.Domain.Gs2Datastore.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Deploy":
                        Gs2Deploy.Domain.Gs2Deploy.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        Gs2Dictionary.Domain.Gs2Dictionary.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Distributor":
                        Gs2Distributor.Domain.Gs2Distributor.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Enchant":
                        Gs2Enchant.Domain.Gs2Enchant.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Enhance":
                        Gs2Enhance.Domain.Gs2Enhance.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Exchange":
                        Gs2Exchange.Domain.Gs2Exchange.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Experience":
                        Gs2Experience.Domain.Gs2Experience.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Formation":
                        Gs2Formation.Domain.Gs2Formation.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Friend":
                        Gs2Friend.Domain.Gs2Friend.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Gateway":
                        Gs2Gateway.Domain.Gs2Gateway.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Identifier":
                        Gs2Identifier.Domain.Gs2Identifier.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Idle":
                        Gs2Idle.Domain.Gs2Idle.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Inbox":
                        Gs2Inbox.Domain.Gs2Inbox.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Inventory":
                        Gs2Inventory.Domain.Gs2Inventory.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        Gs2JobQueue.Domain.Gs2JobQueue.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Key":
                        Gs2Key.Domain.Gs2Key.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Limit":
                        Gs2Limit.Domain.Gs2Limit.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        Gs2LoginReward.Domain.Gs2LoginReward.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Lock":
                        Gs2Lock.Domain.Gs2Lock.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Log":
                        Gs2Log.Domain.Gs2Log.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Lottery":
                        Gs2Lottery.Domain.Gs2Lottery.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        Gs2Matchmaking.Domain.Gs2Matchmaking.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2MegaField":
                        Gs2MegaField.Domain.Gs2MegaField.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Mission":
                        Gs2Mission.Domain.Gs2Mission.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Money":
                        Gs2Money.Domain.Gs2Money.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2News":
                        Gs2News.Domain.Gs2News.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Quest":
                        Gs2Quest.Domain.Gs2Quest.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Ranking":
                        Gs2Ranking.Domain.Gs2Ranking.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Realtime":
                        Gs2Realtime.Domain.Gs2Realtime.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Schedule":
                        Gs2Schedule.Domain.Gs2Schedule.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Script":
                        Gs2Script.Domain.Gs2Script.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        Gs2SerialKey.Domain.Gs2SerialKey.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Showcase":
                        Gs2Showcase.Domain.Gs2Showcase.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        Gs2SkillTree.Domain.Gs2SkillTree.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Stamina":
                        Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        Gs2StateMachine.Domain.Gs2StateMachine.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                    case "Gs2Version":
                        Gs2Version.Domain.Gs2Version.UpdateCacheFromStampSheet(cache, transactionId, method, request, result);
                        break;
                }
            }
        }

        public static void UpdateCacheFromStampTask(
            CacheDatabase cache,
            string taskId,
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
                        Gs2Account.Domain.Gs2Account.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2AdReward":
                        Gs2AdReward.Domain.Gs2AdReward.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Auth":
                        Gs2Auth.Domain.Gs2Auth.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Chat":
                        Gs2Chat.Domain.Gs2Chat.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Datastore":
                        Gs2Datastore.Domain.Gs2Datastore.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Deploy":
                        Gs2Deploy.Domain.Gs2Deploy.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        Gs2Dictionary.Domain.Gs2Dictionary.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Distributor":
                        Gs2Distributor.Domain.Gs2Distributor.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Enchant":
                        Gs2Enchant.Domain.Gs2Enchant.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Enhance":
                        Gs2Enhance.Domain.Gs2Enhance.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Exchange":
                        Gs2Exchange.Domain.Gs2Exchange.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Experience":
                        Gs2Experience.Domain.Gs2Experience.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Formation":
                        Gs2Formation.Domain.Gs2Formation.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Friend":
                        Gs2Friend.Domain.Gs2Friend.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Gateway":
                        Gs2Gateway.Domain.Gs2Gateway.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Identifier":
                        Gs2Identifier.Domain.Gs2Identifier.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Idle":
                        Gs2Idle.Domain.Gs2Idle.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Inbox":
                        Gs2Inbox.Domain.Gs2Inbox.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Inventory":
                        Gs2Inventory.Domain.Gs2Inventory.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        Gs2JobQueue.Domain.Gs2JobQueue.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Key":
                        Gs2Key.Domain.Gs2Key.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Limit":
                        Gs2Limit.Domain.Gs2Limit.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        Gs2LoginReward.Domain.Gs2LoginReward.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Lock":
                        Gs2Lock.Domain.Gs2Lock.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Log":
                        Gs2Log.Domain.Gs2Log.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Lottery":
                        Gs2Lottery.Domain.Gs2Lottery.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        Gs2Matchmaking.Domain.Gs2Matchmaking.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2MegaField":
                        Gs2MegaField.Domain.Gs2MegaField.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Mission":
                        Gs2Mission.Domain.Gs2Mission.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Money":
                        Gs2Money.Domain.Gs2Money.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2News":
                        Gs2News.Domain.Gs2News.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Quest":
                        Gs2Quest.Domain.Gs2Quest.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Ranking":
                        Gs2Ranking.Domain.Gs2Ranking.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Realtime":
                        Gs2Realtime.Domain.Gs2Realtime.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Schedule":
                        Gs2Schedule.Domain.Gs2Schedule.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Script":
                        Gs2Script.Domain.Gs2Script.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        Gs2SerialKey.Domain.Gs2SerialKey.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Showcase":
                        Gs2Showcase.Domain.Gs2Showcase.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        Gs2SkillTree.Domain.Gs2SkillTree.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Stamina":
                        Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        Gs2StateMachine.Domain.Gs2StateMachine.UpdateCacheFromStampTask(cache, taskId, method, request, result);
                        break;
                    case "Gs2Version":
                        Gs2Version.Domain.Gs2Version.UpdateCacheFromStampTask(cache, taskId, method, request, result);
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
                            case "ad_reward":
                                Gs2AdReward.Domain.Gs2AdReward.UpdateCacheFromJobResult(cache, method, job, result);
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
                            case "enchant":
                                Gs2Enchant.Domain.Gs2Enchant.UpdateCacheFromJobResult(cache, method, job, result);
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
                            case "skill_tree":
                                Gs2SkillTree.Domain.Gs2SkillTree.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "stamina":
                                Gs2Stamina.Domain.Gs2Stamina.UpdateCacheFromJobResult(cache, method, job, result);
                                break;
                            case "state_machine":
                                Gs2StateMachine.Domain.Gs2StateMachine.UpdateCacheFromJobResult(cache, method, job, result);
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