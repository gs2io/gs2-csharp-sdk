using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
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
        private readonly TransactionConfiguration _sheetConfiguration;
        private readonly Gs2RestSession _restSession;
        private readonly Gs2WebSocketSession _webSocketSession;

        internal CacheDatabase Cache => this._cache;
        internal JobQueueDomain JobQueueDomain => this._jobQueueDomain;
        internal TransactionConfiguration TransactionConfiguration => this._sheetConfiguration;

        public Gs2RestSession RestSession => this._restSession;
        public Gs2WebSocketSession WebSocketSession => this._webSocketSession;
        public Model.Region Region => RestSession.Region;

        public readonly Gs2Account.Domain.Gs2Account Account;
        public readonly Gs2AdReward.Domain.Gs2AdReward AdReward;
        public readonly Gs2Auth.Domain.Gs2Auth Auth;
        public readonly Gs2Buff.Domain.Gs2Buff Buff;
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
        public readonly Gs2Grade.Domain.Gs2Grade Grade;
        public readonly Gs2Guard.Domain.Gs2Guard Guard;
        public readonly Gs2Guild.Domain.Gs2Guild Guild;
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
        public readonly Gs2Money2.Domain.Gs2Money2 Money2;
        public readonly Gs2News.Domain.Gs2News News;
        public readonly Gs2Quest.Domain.Gs2Quest Quest;
        public readonly Gs2Ranking.Domain.Gs2Ranking Ranking;
        public readonly Gs2Ranking2.Domain.Gs2Ranking2 Ranking2;
        public readonly Gs2Realtime.Domain.Gs2Realtime Realtime;
        public readonly Gs2Schedule.Domain.Gs2Schedule Schedule;
        public readonly Gs2Script.Domain.Gs2Script Script;
        public readonly Gs2SeasonRating.Domain.Gs2SeasonRating SeasonRating;
        public readonly Gs2SerialKey.Domain.Gs2SerialKey SerialKey;
        public readonly Gs2Showcase.Domain.Gs2Showcase Showcase;
        public readonly Gs2SkillTree.Domain.Gs2SkillTree SkillTree;
        public readonly Gs2Stamina.Domain.Gs2Stamina Stamina;
        public readonly Gs2StateMachine.Domain.Gs2StateMachine StateMachine;
        public readonly Gs2Version.Domain.Gs2Version Version;

        public string DistributorNamespaceName => this._sheetConfiguration.NamespaceName;
        public string DefaultContextStack { get; set; }

        public Gs2(
            Gs2RestSession session,
            Gs2WebSocketSession wssession = null,
            string distributorNamespaceName = null
        )
        {
            this._sheetConfiguration = TransactionConfiguration.Builder()
                .WithNamespaceName(distributorNamespaceName)
                .WithConsumeActionEventHandler(UpdateCacheFromConsumeAction)
                .WithAcquireActionEventHandler(UpdateCacheFromAcquireAction)
                .Build();
            this._restSession = session;
            this._webSocketSession = wssession;
            this._cache = new CacheDatabase();
            this._jobQueueDomain = new JobQueueDomain(this);

            this.Account = new Gs2Account.Domain.Gs2Account(this);
            this.AdReward = new Gs2AdReward.Domain.Gs2AdReward(this);
            this.Auth = new Gs2Auth.Domain.Gs2Auth(this);
            this.Buff = new Gs2Buff.Domain.Gs2Buff(this);
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
            this.Grade = new Gs2Grade.Domain.Gs2Grade(this);
            this.Guard = new Gs2Guard.Domain.Gs2Guard(this);
            this.Guild = new Gs2Guild.Domain.Gs2Guild(this);
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
            this.Money2 = new Gs2Money2.Domain.Gs2Money2(this);
            this.News = new Gs2News.Domain.Gs2News(this);
            this.Quest = new Gs2Quest.Domain.Gs2Quest(this);
            this.Ranking = new Gs2Ranking.Domain.Gs2Ranking(this);
            this.Ranking2 = new Gs2Ranking2.Domain.Gs2Ranking2(this);
            this.Realtime = new Gs2Realtime.Domain.Gs2Realtime(this);
            this.Schedule = new Gs2Schedule.Domain.Gs2Schedule(this);
            this.Script = new Gs2Script.Domain.Gs2Script(this);
            this.SeasonRating = new Gs2SeasonRating.Domain.Gs2SeasonRating(this);
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
                            case "Gs2Buff":
                                this.Buff.HandleNotification(this._cache, method, message.payload);
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
                            case "Gs2Grade":
                                this.Grade.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Guard":
                                this.Guard.HandleNotification(this._cache, method, message.payload);
                                break;
                            case "Gs2Guild":
                                this.Guild.HandleNotification(this._cache, method, message.payload);
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
                            case "Gs2Money2":
                                this.Money2.HandleNotification(this._cache, method, message.payload);
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
                            case "Gs2Ranking2":
                                this.Ranking2.HandleNotification(this._cache, method, message.payload);
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
                            case "Gs2SeasonRating":
                                this.SeasonRating.HandleNotification(this._cache, method, message.payload);
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
        public Gs2Future<bool> DispatchFuture(
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
                        var future = this.Distributor.DispatchFuture(
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
                        var future = this.JobQueue.DispatchFuture(
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
                        var future = this._jobQueueDomain.RunFuture(
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

        public Gs2Future DispatchByUserIdFuture(
            string userId
        )
        {
            IEnumerator Impl(Gs2Future self)
            {
                if (DateTime.Now - _lastPingAt > TimeSpan.FromMinutes(5))
                {
                    _webSocketSession?.Ping();
                    _lastPingAt = DateTime.Now;
                }
                
                {
                    var future = this.Distributor.DispatchByUserIdFuture(
                        userId
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                {
                    var future = this.JobQueue.DispatchByUserIdFuture(
                        userId
                    );
                    yield return future;
                    if (future.Error != null)
                    {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                while (true)
                {
                    var future = this._jobQueueDomain.RunByUserIdFuture(
                        userId
                    );
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                    if (future.Result) {
                        break;
                    }
                }
            }

            return new Gs2InlineFuture(Impl);
        }
#endif

#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchAsync(
    #else
        public async Task DispatchAsync(
    #endif
            AccessToken accessToken
        )
        {
            if (DateTime.Now - _lastPingAt > TimeSpan.FromMinutes(5))
            {
                _webSocketSession?.Ping();
                _lastPingAt = DateTime.Now;
            }

            await this.Distributor.DispatchAsync(
                accessToken
            );

            await this.JobQueue.DispatchAsync(
                accessToken
            );

            while (true)
            {
                if (await this._jobQueueDomain.RunAsync(
                        accessToken
                    ))
                {
                    break;
                }
            }
        }

    #if UNITY_2017_1_OR_NEWER
        public async UniTask DispatchByUserIdAsync(
    #else
        public async Task DispatchByUserIdAsync(
    #endif
            string userId
        )
        {
            if (DateTime.Now - _lastPingAt > TimeSpan.FromMinutes(5))
            {
                _webSocketSession?.Ping();
                _lastPingAt = DateTime.Now;
            }

            await this.Distributor.DispatchByUserIdAsync(
                userId
            );

            await this.JobQueue.DispatchByUserIdAsync(
                userId
            );

            while (true)
            {
                if (await this._jobQueueDomain.RunByUserIdAsync(
                        userId
                    ))
                {
                    break;
                }
            }
        }
#endif

        public void UpdateCacheFromAcquireAction(
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

            if (action.Contains(":"))
            {
                var service = action.Substring(0, action.IndexOf(':'));
                var method = action.Substring(action.IndexOf(':') + 1);
                switch (service)
                {
                    case "Gs2Account":
                        this.Account.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2AdReward":
                        this.AdReward.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Auth":
                        this.Auth.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Buff":
                        this.Buff.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Chat":
                        this.Chat.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Datastore":
                        this.Datastore.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Deploy":
                        this.Deploy.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        this.Dictionary.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Distributor":
                        this.Distributor.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Enchant":
                        this.Enchant.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Enhance":
                        this.Enhance.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Exchange":
                        this.Exchange.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Experience":
                        this.Experience.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Formation":
                        this.Formation.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Friend":
                        this.Friend.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Gateway":
                        this.Gateway.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Grade":
                        this.Grade.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Guard":
                        this.Guard.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Guild":
                        this.Guild.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Identifier":
                        this.Identifier.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Idle":
                        this.Idle.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Inbox":
                        this.Inbox.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Inventory":
                        this.Inventory.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        this.JobQueue.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Key":
                        this.Key.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Limit":
                        this.Limit.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        this.LoginReward.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Lock":
                        this.Lock.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Log":
                        this.Log.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Lottery":
                        this.Lottery.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        this.Matchmaking.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2MegaField":
                        this.MegaField.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Mission":
                        this.Mission.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Money":
                        this.Money.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Money2":
                        this.Money2.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2News":
                        this.News.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Quest":
                        this.Quest.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Ranking":
                        this.Ranking.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Ranking2":
                        this.Ranking2.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Realtime":
                        this.Realtime.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Schedule":
                        this.Schedule.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Script":
                        this.Script.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2SeasonRating":
                        this.SeasonRating.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        this.SerialKey.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Showcase":
                        this.Showcase.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        this.SkillTree.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Stamina":
                        this.Stamina.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        this.StateMachine.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                    case "Gs2Version":
                        this.Version.UpdateCacheFromStampSheet(transactionId, method, request, result);
                        break;
                }
            }
        }

        public void UpdateCacheFromConsumeAction(
            CacheDatabase cache,
            string taskId,
            string action,
            string request,
            string result
        )
        {
            if (action.Contains(":"))
            {
                var service = action.Substring(0, action.IndexOf(':'));
                var method = action.Substring(action.IndexOf(':') + 1);
                switch (service)
                {
                    case "Gs2Account":
                        this.Account.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2AdReward":
                        this.AdReward.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Auth":
                        this.Auth.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Buff":
                        this.Buff.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Chat":
                        this.Chat.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Datastore":
                        this.Datastore.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Deploy":
                        this.Deploy.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        this.Dictionary.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Distributor":
                        this.Distributor.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Enchant":
                        this.Enchant.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Enhance":
                        this.Enhance.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Exchange":
                        this.Exchange.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Experience":
                        this.Experience.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Formation":
                        this.Formation.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Friend":
                        this.Friend.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Gateway":
                        this.Gateway.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Grade":
                        this.Grade.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Guard":
                        this.Guard.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Guild":
                        this.Guild.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Identifier":
                        this.Identifier.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Idle":
                        this.Idle.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Inbox":
                        this.Inbox.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Inventory":
                        this.Inventory.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        this.JobQueue.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Key":
                        this.Key.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Limit":
                        this.Limit.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        this.LoginReward.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Lock":
                        this.Lock.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Log":
                        this.Log.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Lottery":
                        this.Lottery.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        this.Matchmaking.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2MegaField":
                        this.MegaField.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Mission":
                        this.Mission.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Money":
                        this.Money.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Money2":
                        this.Money2.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2News":
                        this.News.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Quest":
                        this.Quest.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Ranking":
                        this.Ranking.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Ranking2":
                        this.Ranking2.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Realtime":
                        this.Realtime.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Schedule":
                        this.Schedule.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Script":
                        this.Script.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2SeasonRating":
                        this.SeasonRating.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        this.SerialKey.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Showcase":
                        this.Showcase.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        this.SkillTree.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Stamina":
                        this.Stamina.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        this.StateMachine.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                    case "Gs2Version":
                        this.Version.UpdateCacheFromStampTask(taskId, method, request, result);
                        break;
                }
            }
        }

        public void PushJobQueue(
            string namespaceName
        )
        {
            this._jobQueueDomain.Push(namespaceName);
        }

        public void UpdateCacheFromJobResult(
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
                                this.Account.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "ad_reward":
                                this.AdReward.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "auth":
                                this.Auth.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "buff":
                                this.Buff.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "chat":
                                this.Chat.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "datastore":
                                this.Datastore.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "deploy":
                                this.Deploy.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "dictionary":
                                this.Dictionary.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "distributor":
                                this.Distributor.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "enchant":
                                this.Enchant.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "enhance":
                                this.Enhance.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "exchange":
                                this.Exchange.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "experience":
                                this.Experience.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "formation":
                                this.Formation.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "friend":
                                this.Friend.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "gateway":
                                this.Gateway.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "grade":
                                this.Grade.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "guard":
                                this.Guard.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "guild":
                                this.Guild.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "identifier":
                                this.Identifier.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "idle":
                                this.Idle.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "inbox":
                                this.Inbox.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "inventory":
                                this.Inventory.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "job_queue":
                                this.JobQueue.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "key":
                                this.Key.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "limit":
                                this.Limit.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "login_reward":
                                this.LoginReward.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "lock":
                                this.Lock.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "log":
                                this.Log.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "lottery":
                                this.Lottery.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "matchmaking":
                                this.Matchmaking.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "mega_field":
                                this.MegaField.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "mission":
                                this.Mission.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "money":
                                this.Money.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "money2":
                                this.Money2.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "news":
                                this.News.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "quest":
                                this.Quest.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "ranking":
                                this.Ranking.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "ranking2":
                                this.Ranking2.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "realtime":
                                this.Realtime.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "schedule":
                                this.Schedule.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "script":
                                this.Script.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "season_rating":
                                this.SeasonRating.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "serial_key":
                                this.SerialKey.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "showcase":
                                this.Showcase.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "skill_tree":
                                this.SkillTree.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "stamina":
                                this.Stamina.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "state_machine":
                                this.StateMachine.UpdateCacheFromJobResult(method, job, result);
                                break;
                            case "version":
                                this.Version.UpdateCacheFromJobResult(method, job, result);
                                break;
                        }
                    }
                }
            }
        }
        
        public Gs2Future DisconnectFuture()
        {
            IEnumerator Impl(Gs2Future self)
            {
                {
                    var future = this._restSession.CloseFuture();
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                {
                    var future = this._webSocketSession.CloseFuture();
                    yield return future;
                    if (future.Error != null) {
                        self.OnError(future.Error);
                        yield break;
                    }
                }
                self.OnComplete(null);
            }
            return new Gs2InlineFuture(Impl);
        }
        
#if GS2_ENABLE_UNITASK
        public async UniTask DisconnectAsync()
        {
            await this._restSession.CloseAsync();
            await this._webSocketSession.CloseAsync();
        }
#endif
    }
}