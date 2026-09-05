#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using Gs2.Core.Net;
using Gs2.Core.Util;
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
        private static readonly string[] MultiWordJobResultServices =
        {
            "state_machine",
            "season_rating",
            "login_reward",
            "skill_tree",
            "serial_key",
            "mega_field",
            "job_queue",
            "ad_reward",
        };

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
        public readonly Gs2Freeze.Domain.Gs2Freeze Freeze;
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
                .WithVerifyActionEventHandler(UpdateCacheFromConsumeAction)
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
            this.Freeze = new Gs2Freeze.Domain.Gs2Freeze(this);
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
                var weakSelf = new WeakReference(this);
                Gs2WebSocketSession.NotificationHandler notificationHandler = null;
                notificationHandler = message =>
                {
                    var target = weakSelf.Target as Gs2;
                    if (target == null)
                    {
                        wssession.OnSdkNotificationMessage -= notificationHandler;
                        return;
                    }
                    if (message.subject.Contains(":"))
                    {
                        var service = message.subject.Substring(0, message.subject.IndexOf(':'));
                        var method = message.subject.Substring(message.subject.IndexOf(':') + 1);
                        switch (service)
                        {
                            case "Gs2Account":
                                target.Account.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2AdReward":
                                target.AdReward.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Auth":
                                target.Auth.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Buff":
                                target.Buff.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Chat":
                                target.Chat.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Datastore":
                                target.Datastore.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Deploy":
                                target.Deploy.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Dictionary":
                                target.Dictionary.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Distributor":
                                target.Distributor.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Enchant":
                                target.Enchant.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Enhance":
                                target.Enhance.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Exchange":
                                target.Exchange.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Experience":
                                target.Experience.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Formation":
                                target.Formation.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Freeze":
                                target.Freeze.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Friend":
                                target.Friend.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Gateway":
                                target.Gateway.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Grade":
                                target.Grade.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Guard":
                                target.Guard.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Guild":
                                target.Guild.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Identifier":
                                target.Identifier.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Idle":
                                target.Idle.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Inbox":
                                target.Inbox.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Inventory":
                                target.Inventory.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2JobQueue":
                                target.JobQueue.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Key":
                                target.Key.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Limit":
                                target.Limit.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2LoginReward":
                                target.LoginReward.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Lock":
                                target.Lock.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Log":
                                target.Log.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Lottery":
                                target.Lottery.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Matchmaking":
                                target.Matchmaking.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2MegaField":
                                target.MegaField.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Mission":
                                target.Mission.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Money":
                                target.Money.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Money2":
                                target.Money2.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2News":
                                target.News.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Quest":
                                target.Quest.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Ranking":
                                target.Ranking.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Ranking2":
                                target.Ranking2.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Realtime":
                                target.Realtime.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Schedule":
                                target.Schedule.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Script":
                                target.Script.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2SeasonRating":
                                target.SeasonRating.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2SerialKey":
                                target.SerialKey.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Showcase":
                                target.Showcase.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2SkillTree":
                                target.SkillTree.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Stamina":
                                target.Stamina.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2StateMachine":
                                target.StateMachine.HandleNotification(target._cache, method, message.payload);
                                break;
                            case "Gs2Version":
                                target.Version.HandleNotification(target._cache, method, message.payload);
                                break;
                        }
                    }
                };
                wssession.OnSdkNotificationMessage += notificationHandler;
            }
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public void ClearCacheAndAllUnsubscribe()
        {
            _cache.ClearAndAllUnsubscribe();
        }

        public void ClearCache<TKind>(
            string parentKey,
            string key
        ) {
            _cache.Delete<TKind>(parentKey, key);
        }
#if UNITY_2017_1_OR_NEWER
        public Gs2Future DispatchFuture(
            AccessToken accessToken
        ) => DispatchAsync(accessToken).ToGs2Future();

        public Gs2Future DispatchByUserIdFuture(
            string userId
        ) => DispatchByUserIdAsync(userId).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
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

#if GS2_ENABLE_UNITASK
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

        public void UpdateCacheFromAcquireAction(
            CacheDatabase cache,
            string transactionId,
            int? timeOffset,
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
                        this.Account.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2AdReward":
                        this.AdReward.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Auth":
                        this.Auth.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Buff":
                        this.Buff.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Chat":
                        this.Chat.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Datastore":
                        this.Datastore.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Deploy":
                        this.Deploy.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        this.Dictionary.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Distributor":
                        this.Distributor.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Enchant":
                        this.Enchant.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Enhance":
                        this.Enhance.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Exchange":
                        this.Exchange.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Experience":
                        this.Experience.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Formation":
                        this.Formation.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Freeze":
                        this.Freeze.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Friend":
                        this.Friend.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Gateway":
                        this.Gateway.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Grade":
                        this.Grade.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Guard":
                        this.Guard.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Guild":
                        this.Guild.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Identifier":
                        this.Identifier.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Idle":
                        this.Idle.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Inbox":
                        this.Inbox.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Inventory":
                        this.Inventory.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        this.JobQueue.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Key":
                        this.Key.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Limit":
                        this.Limit.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        this.LoginReward.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Lock":
                        this.Lock.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Log":
                        this.Log.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Lottery":
                        this.Lottery.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        this.Matchmaking.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2MegaField":
                        this.MegaField.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Mission":
                        this.Mission.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Money":
                        this.Money.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Money2":
                        this.Money2.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2News":
                        this.News.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Quest":
                        this.Quest.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Ranking":
                        this.Ranking.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Ranking2":
                        this.Ranking2.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Realtime":
                        this.Realtime.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Schedule":
                        this.Schedule.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Script":
                        this.Script.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2SeasonRating":
                        this.SeasonRating.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        this.SerialKey.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Showcase":
                        this.Showcase.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        this.SkillTree.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Stamina":
                        this.Stamina.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        this.StateMachine.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                    case "Gs2Version":
                        this.Version.UpdateCacheFromStampSheet(transactionId, timeOffset, method, request, result);
                        break;
                }
            }
        }

        public void UpdateCacheFromConsumeAction(
            CacheDatabase cache,
            string taskId,
            int? timeOffset,
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
                        this.Account.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2AdReward":
                        this.AdReward.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Auth":
                        this.Auth.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Buff":
                        this.Buff.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Chat":
                        this.Chat.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Datastore":
                        this.Datastore.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Deploy":
                        this.Deploy.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Dictionary":
                        this.Dictionary.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Distributor":
                        this.Distributor.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Enchant":
                        this.Enchant.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Enhance":
                        this.Enhance.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Exchange":
                        this.Exchange.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Experience":
                        this.Experience.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Formation":
                        this.Formation.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Freeze":
                        this.Freeze.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Friend":
                        this.Friend.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Gateway":
                        this.Gateway.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Grade":
                        this.Grade.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Guard":
                        this.Guard.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Guild":
                        this.Guild.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Identifier":
                        this.Identifier.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Idle":
                        this.Idle.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Inbox":
                        this.Inbox.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Inventory":
                        this.Inventory.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2JobQueue":
                        this.JobQueue.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Key":
                        this.Key.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Limit":
                        this.Limit.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2LoginReward":
                        this.LoginReward.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Lock":
                        this.Lock.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Log":
                        this.Log.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Lottery":
                        this.Lottery.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Matchmaking":
                        this.Matchmaking.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2MegaField":
                        this.MegaField.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Mission":
                        this.Mission.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Money":
                        this.Money.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Money2":
                        this.Money2.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2News":
                        this.News.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Quest":
                        this.Quest.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Ranking":
                        this.Ranking.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Ranking2":
                        this.Ranking2.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Realtime":
                        this.Realtime.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Schedule":
                        this.Schedule.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Script":
                        this.Script.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2SeasonRating":
                        this.SeasonRating.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2SerialKey":
                        this.SerialKey.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Showcase":
                        this.Showcase.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2SkillTree":
                        this.SkillTree.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Stamina":
                        this.Stamina.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2StateMachine":
                        this.StateMachine.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
                        break;
                    case "Gs2Version":
                        this.Version.UpdateCacheFromStampTask(taskId, timeOffset, method, request, result);
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

        internal static bool TryParseJobResultScriptName(
            string scriptName,
            out string service,
            out string method
        )
        {
            service = null;
            method = null;
            const string prefix = "execute_";
            if (scriptName == null || !scriptName.StartsWith(prefix, StringComparison.Ordinal))
            {
                return false;
            }

            var actionName = scriptName.Substring(prefix.Length);
            foreach (var candidate in MultiWordJobResultServices)
            {
                var servicePrefix = candidate + "_";
                if (actionName.StartsWith(servicePrefix, StringComparison.Ordinal))
                {
                    var parsedMethod = actionName.Substring(servicePrefix.Length);
                    if (parsedMethod.Length == 0)
                    {
                        return false;
                    }
                    service = candidate;
                    method = parsedMethod;
                    return true;
                }
            }

            var separatorIndex = actionName.IndexOf('_');
            if (separatorIndex <= 0 || separatorIndex == actionName.Length - 1)
            {
                return false;
            }
            service = actionName.Substring(0, separatorIndex);
            method = actionName.Substring(separatorIndex + 1);
            return true;
        }

        public void UpdateCacheFromJobResult(
            int? timeOffset,
            Job job,
            JobResultBody result
        )
        {
            if (job.ScriptId.Split(':').Length > 4)
            {
                if (job.ScriptId.Split(':')[3] == "system")
                {
                    var scriptName = job.ScriptId.Substring(job.ScriptId.LastIndexOf(':') + 1);
                    if (TryParseJobResultScriptName(scriptName, out var service, out var method))
                    {
                        switch (service)
                        {
                            case "account":
                                this.Account.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "ad_reward":
                                this.AdReward.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "auth":
                                this.Auth.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "buff":
                                this.Buff.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "chat":
                                this.Chat.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "datastore":
                                this.Datastore.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "deploy":
                                this.Deploy.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "dictionary":
                                this.Dictionary.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "distributor":
                                this.Distributor.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "enchant":
                                this.Enchant.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "enhance":
                                this.Enhance.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "exchange":
                                this.Exchange.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "experience":
                                this.Experience.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "formation":
                                this.Formation.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "freeze":
                                this.Freeze.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "friend":
                                this.Friend.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "gateway":
                                this.Gateway.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "grade":
                                this.Grade.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "guard":
                                this.Guard.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "guild":
                                this.Guild.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "identifier":
                                this.Identifier.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "idle":
                                this.Idle.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "inbox":
                                this.Inbox.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "inventory":
                                this.Inventory.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "job_queue":
                                this.JobQueue.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "key":
                                this.Key.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "limit":
                                this.Limit.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "login_reward":
                                this.LoginReward.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "lock":
                                this.Lock.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "log":
                                this.Log.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "lottery":
                                this.Lottery.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "matchmaking":
                                this.Matchmaking.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "mega_field":
                                this.MegaField.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "mission":
                                this.Mission.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "money":
                                this.Money.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "money2":
                                this.Money2.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "news":
                                this.News.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "quest":
                                this.Quest.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "ranking":
                                this.Ranking.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "ranking2":
                                this.Ranking2.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "realtime":
                                this.Realtime.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "schedule":
                                this.Schedule.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "script":
                                this.Script.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "season_rating":
                                this.SeasonRating.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "serial_key":
                                this.SerialKey.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "showcase":
                                this.Showcase.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "skill_tree":
                                this.SkillTree.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "stamina":
                                this.Stamina.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "state_machine":
                                this.StateMachine.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                            case "version":
                                this.Version.UpdateCacheFromJobResult(method, timeOffset, job, result);
                                break;
                        }
                    }
                }
            }
        }
        
        public Gs2Future DisconnectFuture() => DisconnectAsync().ToGs2Future();
        
#if GS2_ENABLE_UNITASK
        public async UniTask DisconnectAsync()
#else
        public async Task DisconnectAsync()
#endif
        {
            await this._restSession.CloseAsync();
            if (this._webSocketSession != null)
            {
                await this._webSocketSession.CloseAsync();
            }
        }
    }
}
