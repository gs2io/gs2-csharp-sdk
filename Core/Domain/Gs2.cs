#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
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

        // ---------------------------------------------------------------------------------------------
        // ユーザーの全データの一括取得（Gs2Distributor:DescribeUserData）でキャッシュを作る
        //
        // ★ログイン直後に 1 回 await すると、そのユーザーの全 GS2 サービスのデータ（スタミナ・インベントリ・
        //   ミッション進捗 …）が各モデルのキャッシュに載り、以後の Get / Describe はサーバーへ出ない。
        //   各エントリは kind でモデルを示し、サービスごとの生成物 `Gs2<Service>.Model.Cache.Gs2<Service>.PutUserData`
        //   （kind → モデルの振り分け。鍵の取り出しは各モデルの `PutUserData`、sdk-gen の BaseModel.user_data_cache_keys）
        //   へ渡す。キー方式 v2 のプロジェクトでだけ使える（v1 は BadRequest）。
        //
        // ★「リストが揃った印」（Describe のイテレータがサーバーへ出ない条件）は、全ページを読み終えてから
        //   (service, kind, 親キー) の集合にまとめて立てる。途中で失敗したら印は立てない（入れた item は個別 Get の
        //   キャッシュとして残る）。エントリ単位の JSON の失敗は数えず続行する。
        //   ロード中に作ったイテレータは部分的なリストを返しうるので、他の呼び出しの前に await すること。
        //
        // 戻り値: キャッシュに入れたエントリ数。知らない service / kind（SDK が古い、または対応表に無い）は数えず捨てる。
        // ---------------------------------------------------------------------------------------------

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<int> LoadUserDataFuture(
            AccessToken accessToken
        ) => LoadUserDataAsync(accessToken).ToGs2Future();
#endif

#if GS2_ENABLE_UNITASK
        public async UniTask<int> LoadUserDataAsync(
#else
        public async Task<int> LoadUserDataAsync(
#endif
            AccessToken accessToken
        )
        {
            if (accessToken == null) {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var client = new Gs2Distributor.Gs2DistributorRestClient(RestSession);
            var loaded = 0;
            var listCached = new HashSet<(string service, string kind, string parentKey)>();
            string pageToken = null;
            while (true) {
                var result = await client.DescribeUserDataAsync(
                    new Gs2Distributor.Request.DescribeUserDataRequest()
                        .WithContextStack(DefaultContextStack)
                        .WithAccessToken(accessToken.Token)
                        .WithPageToken(pageToken)
                        .WithLimit(100)
                );
                foreach (var entry in result.Items ?? Array.Empty<Gs2Distributor.Model.UserDataEntry>()) {
                    string parentKey;
                    try {
                        parentKey = PutUserData(entry.Service, entry.NamespaceName, accessToken.UserId, accessToken.TimeOffset, entry.Kind, entry.Payload);
                    }
                    catch (System.Exception) {
                        // 1 件の JSON が読めなくても他のエントリは入れる（個別 API で取り直せる）
                        continue;
                    }
                    if (parentKey == null) {
                        continue;
                    }
                    loaded++;
                    listCached.Add((entry.Service, entry.Kind, parentKey));
                }
                pageToken = result.NextPageToken;
                if (string.IsNullOrEmpty(pageToken)) {
                    break;
                }
            }
            foreach (var (service, kind, parentKey) in listCached) {
                SetListCached(service, accessToken.TimeOffset, kind, parentKey);
            }
            return loaded;
        }

        /// <summary>
        /// 一括取得の 1 エントリを、service の生成物へ振り分けてキャッシュへ入れる。戻り値は親キー（知らない service / kind は null）。
        /// service は seed のディレクトリ名の綴り（"stamina" / "skill_tree"）。
        /// </summary>
        public string PutUserData(
            string service,
            string namespaceName,
            string userId,
            int? timeOffset,
            string kind,
            string payload
        ) {
            switch (service) {
                case "account":
                    return Gs2Account.Model.Cache.Gs2Account.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "ad_reward":
                    return Gs2AdReward.Model.Cache.Gs2AdReward.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "auth":
                    return Gs2Auth.Model.Cache.Gs2Auth.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "buff":
                    return Gs2Buff.Model.Cache.Gs2Buff.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "chat":
                    return Gs2Chat.Model.Cache.Gs2Chat.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "datastore":
                    return Gs2Datastore.Model.Cache.Gs2Datastore.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "deploy":
                    return Gs2Deploy.Model.Cache.Gs2Deploy.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "dictionary":
                    return Gs2Dictionary.Model.Cache.Gs2Dictionary.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "distributor":
                    return Gs2Distributor.Model.Cache.Gs2Distributor.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "enchant":
                    return Gs2Enchant.Model.Cache.Gs2Enchant.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "enhance":
                    return Gs2Enhance.Model.Cache.Gs2Enhance.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "exchange":
                    return Gs2Exchange.Model.Cache.Gs2Exchange.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "experience":
                    return Gs2Experience.Model.Cache.Gs2Experience.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "formation":
                    return Gs2Formation.Model.Cache.Gs2Formation.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "freeze":
                    return Gs2Freeze.Model.Cache.Gs2Freeze.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "friend":
                    return Gs2Friend.Model.Cache.Gs2Friend.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "gateway":
                    return Gs2Gateway.Model.Cache.Gs2Gateway.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "grade":
                    return Gs2Grade.Model.Cache.Gs2Grade.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "guard":
                    return Gs2Guard.Model.Cache.Gs2Guard.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "guild":
                    return Gs2Guild.Model.Cache.Gs2Guild.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "identifier":
                    return Gs2Identifier.Model.Cache.Gs2Identifier.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "idle":
                    return Gs2Idle.Model.Cache.Gs2Idle.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "inbox":
                    return Gs2Inbox.Model.Cache.Gs2Inbox.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "inventory":
                    return Gs2Inventory.Model.Cache.Gs2Inventory.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "job_queue":
                    return Gs2JobQueue.Model.Cache.Gs2JobQueue.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "key":
                    return Gs2Key.Model.Cache.Gs2Key.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "limit":
                    return Gs2Limit.Model.Cache.Gs2Limit.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "lock":
                    return Gs2Lock.Model.Cache.Gs2Lock.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "log":
                    return Gs2Log.Model.Cache.Gs2Log.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "login_reward":
                    return Gs2LoginReward.Model.Cache.Gs2LoginReward.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "lottery":
                    return Gs2Lottery.Model.Cache.Gs2Lottery.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "matchmaking":
                    return Gs2Matchmaking.Model.Cache.Gs2Matchmaking.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "mega_field":
                    return Gs2MegaField.Model.Cache.Gs2MegaField.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "mission":
                    return Gs2Mission.Model.Cache.Gs2Mission.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "money":
                    return Gs2Money.Model.Cache.Gs2Money.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "money2":
                    return Gs2Money2.Model.Cache.Gs2Money2.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "news":
                    return Gs2News.Model.Cache.Gs2News.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "project":
                    return Gs2Project.Model.Cache.Gs2Project.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "quest":
                    return Gs2Quest.Model.Cache.Gs2Quest.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "ranking":
                    return Gs2Ranking.Model.Cache.Gs2Ranking.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "ranking2":
                    return Gs2Ranking2.Model.Cache.Gs2Ranking2.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "realtime":
                    return Gs2Realtime.Model.Cache.Gs2Realtime.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "schedule":
                    return Gs2Schedule.Model.Cache.Gs2Schedule.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "script":
                    return Gs2Script.Model.Cache.Gs2Script.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "season_rating":
                    return Gs2SeasonRating.Model.Cache.Gs2SeasonRating.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "serial_key":
                    return Gs2SerialKey.Model.Cache.Gs2SerialKey.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "showcase":
                    return Gs2Showcase.Model.Cache.Gs2Showcase.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "skill_tree":
                    return Gs2SkillTree.Model.Cache.Gs2SkillTree.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "stamina":
                    return Gs2Stamina.Model.Cache.Gs2Stamina.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "state_machine":
                    return Gs2StateMachine.Model.Cache.Gs2StateMachine.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                case "version":
                    return Gs2Version.Model.Cache.Gs2Version.PutUserData(_cache, namespaceName, userId, timeOffset, kind, payload);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 一括取得で入れた (service, kind, 親キー) に「リストが揃った印」を立てる。
        /// </summary>
        public bool SetListCached(
            string service,
            int? timeOffset,
            string kind,
            string parentKey
        ) {
            switch (service) {
                case "account":
                    return Gs2Account.Model.Cache.Gs2Account.SetListCached(_cache, timeOffset, kind, parentKey);
                case "ad_reward":
                    return Gs2AdReward.Model.Cache.Gs2AdReward.SetListCached(_cache, timeOffset, kind, parentKey);
                case "auth":
                    return Gs2Auth.Model.Cache.Gs2Auth.SetListCached(_cache, timeOffset, kind, parentKey);
                case "buff":
                    return Gs2Buff.Model.Cache.Gs2Buff.SetListCached(_cache, timeOffset, kind, parentKey);
                case "chat":
                    return Gs2Chat.Model.Cache.Gs2Chat.SetListCached(_cache, timeOffset, kind, parentKey);
                case "datastore":
                    return Gs2Datastore.Model.Cache.Gs2Datastore.SetListCached(_cache, timeOffset, kind, parentKey);
                case "deploy":
                    return Gs2Deploy.Model.Cache.Gs2Deploy.SetListCached(_cache, timeOffset, kind, parentKey);
                case "dictionary":
                    return Gs2Dictionary.Model.Cache.Gs2Dictionary.SetListCached(_cache, timeOffset, kind, parentKey);
                case "distributor":
                    return Gs2Distributor.Model.Cache.Gs2Distributor.SetListCached(_cache, timeOffset, kind, parentKey);
                case "enchant":
                    return Gs2Enchant.Model.Cache.Gs2Enchant.SetListCached(_cache, timeOffset, kind, parentKey);
                case "enhance":
                    return Gs2Enhance.Model.Cache.Gs2Enhance.SetListCached(_cache, timeOffset, kind, parentKey);
                case "exchange":
                    return Gs2Exchange.Model.Cache.Gs2Exchange.SetListCached(_cache, timeOffset, kind, parentKey);
                case "experience":
                    return Gs2Experience.Model.Cache.Gs2Experience.SetListCached(_cache, timeOffset, kind, parentKey);
                case "formation":
                    return Gs2Formation.Model.Cache.Gs2Formation.SetListCached(_cache, timeOffset, kind, parentKey);
                case "freeze":
                    return Gs2Freeze.Model.Cache.Gs2Freeze.SetListCached(_cache, timeOffset, kind, parentKey);
                case "friend":
                    return Gs2Friend.Model.Cache.Gs2Friend.SetListCached(_cache, timeOffset, kind, parentKey);
                case "gateway":
                    return Gs2Gateway.Model.Cache.Gs2Gateway.SetListCached(_cache, timeOffset, kind, parentKey);
                case "grade":
                    return Gs2Grade.Model.Cache.Gs2Grade.SetListCached(_cache, timeOffset, kind, parentKey);
                case "guard":
                    return Gs2Guard.Model.Cache.Gs2Guard.SetListCached(_cache, timeOffset, kind, parentKey);
                case "guild":
                    return Gs2Guild.Model.Cache.Gs2Guild.SetListCached(_cache, timeOffset, kind, parentKey);
                case "identifier":
                    return Gs2Identifier.Model.Cache.Gs2Identifier.SetListCached(_cache, timeOffset, kind, parentKey);
                case "idle":
                    return Gs2Idle.Model.Cache.Gs2Idle.SetListCached(_cache, timeOffset, kind, parentKey);
                case "inbox":
                    return Gs2Inbox.Model.Cache.Gs2Inbox.SetListCached(_cache, timeOffset, kind, parentKey);
                case "inventory":
                    return Gs2Inventory.Model.Cache.Gs2Inventory.SetListCached(_cache, timeOffset, kind, parentKey);
                case "job_queue":
                    return Gs2JobQueue.Model.Cache.Gs2JobQueue.SetListCached(_cache, timeOffset, kind, parentKey);
                case "key":
                    return Gs2Key.Model.Cache.Gs2Key.SetListCached(_cache, timeOffset, kind, parentKey);
                case "limit":
                    return Gs2Limit.Model.Cache.Gs2Limit.SetListCached(_cache, timeOffset, kind, parentKey);
                case "lock":
                    return Gs2Lock.Model.Cache.Gs2Lock.SetListCached(_cache, timeOffset, kind, parentKey);
                case "log":
                    return Gs2Log.Model.Cache.Gs2Log.SetListCached(_cache, timeOffset, kind, parentKey);
                case "login_reward":
                    return Gs2LoginReward.Model.Cache.Gs2LoginReward.SetListCached(_cache, timeOffset, kind, parentKey);
                case "lottery":
                    return Gs2Lottery.Model.Cache.Gs2Lottery.SetListCached(_cache, timeOffset, kind, parentKey);
                case "matchmaking":
                    return Gs2Matchmaking.Model.Cache.Gs2Matchmaking.SetListCached(_cache, timeOffset, kind, parentKey);
                case "mega_field":
                    return Gs2MegaField.Model.Cache.Gs2MegaField.SetListCached(_cache, timeOffset, kind, parentKey);
                case "mission":
                    return Gs2Mission.Model.Cache.Gs2Mission.SetListCached(_cache, timeOffset, kind, parentKey);
                case "money":
                    return Gs2Money.Model.Cache.Gs2Money.SetListCached(_cache, timeOffset, kind, parentKey);
                case "money2":
                    return Gs2Money2.Model.Cache.Gs2Money2.SetListCached(_cache, timeOffset, kind, parentKey);
                case "news":
                    return Gs2News.Model.Cache.Gs2News.SetListCached(_cache, timeOffset, kind, parentKey);
                case "project":
                    return Gs2Project.Model.Cache.Gs2Project.SetListCached(_cache, timeOffset, kind, parentKey);
                case "quest":
                    return Gs2Quest.Model.Cache.Gs2Quest.SetListCached(_cache, timeOffset, kind, parentKey);
                case "ranking":
                    return Gs2Ranking.Model.Cache.Gs2Ranking.SetListCached(_cache, timeOffset, kind, parentKey);
                case "ranking2":
                    return Gs2Ranking2.Model.Cache.Gs2Ranking2.SetListCached(_cache, timeOffset, kind, parentKey);
                case "realtime":
                    return Gs2Realtime.Model.Cache.Gs2Realtime.SetListCached(_cache, timeOffset, kind, parentKey);
                case "schedule":
                    return Gs2Schedule.Model.Cache.Gs2Schedule.SetListCached(_cache, timeOffset, kind, parentKey);
                case "script":
                    return Gs2Script.Model.Cache.Gs2Script.SetListCached(_cache, timeOffset, kind, parentKey);
                case "season_rating":
                    return Gs2SeasonRating.Model.Cache.Gs2SeasonRating.SetListCached(_cache, timeOffset, kind, parentKey);
                case "serial_key":
                    return Gs2SerialKey.Model.Cache.Gs2SerialKey.SetListCached(_cache, timeOffset, kind, parentKey);
                case "showcase":
                    return Gs2Showcase.Model.Cache.Gs2Showcase.SetListCached(_cache, timeOffset, kind, parentKey);
                case "skill_tree":
                    return Gs2SkillTree.Model.Cache.Gs2SkillTree.SetListCached(_cache, timeOffset, kind, parentKey);
                case "stamina":
                    return Gs2Stamina.Model.Cache.Gs2Stamina.SetListCached(_cache, timeOffset, kind, parentKey);
                case "state_machine":
                    return Gs2StateMachine.Model.Cache.Gs2StateMachine.SetListCached(_cache, timeOffset, kind, parentKey);
                case "version":
                    return Gs2Version.Model.Cache.Gs2Version.SetListCached(_cache, timeOffset, kind, parentKey);
                default:
                    return false;
            }
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
