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
 */

using System;
using System.Collections.Generic;
using System.Text;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Exception
{
#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public static class Gs2ErrorResolver
	{
		public static Gs2Exception ResolveAction(string action, Gs2Exception error)
		{
			if (error == null || string.IsNullOrEmpty(action)) {
				return error;
			}
			var separator = action.IndexOf(':');
			if (separator <= 0 || separator == action.Length - 1) {
				return error;
			}
			return Resolve(action.Substring(0, separator), action.Substring(separator + 1), error);
		}

		public static Gs2Exception ResolveJobScript(string scriptId, Gs2Exception error)
		{
			if (error == null || string.IsNullOrEmpty(scriptId)) {
				return error;
			}
			var scriptName = scriptId.Substring(scriptId.LastIndexOf(':') + 1);
			if (!global::Gs2.Core.Domain.Gs2.TryParseJobResultScriptName(scriptName, out var service, out var method)) {
				return error;
			}
			return Resolve("Gs2" + ToUpperCamel(service), ToUpperCamel(method), error);
		}

		public static Gs2Exception ResolveBatch(string service, string methodName, Gs2Exception error)
		{
			if (error == null || string.IsNullOrEmpty(service) || string.IsNullOrEmpty(methodName)) {
				return error;
			}
			return Resolve("Gs2" + ToUpperCamel(service), ToUpperCamel(methodName), error);
		}

		private static string ToUpperCamel(string value)
		{
			var builder = new StringBuilder(value.Length);
			foreach (var part in value.Split('_')) {
				if (part.Length == 0) {
					continue;
				}
				builder.Append(char.ToUpperInvariant(part[0]));
				builder.Append(part, 1, part.Length - 1);
			}
			return builder.ToString();
		}

		private static readonly Dictionary<string, Func<string, Gs2Exception, Gs2Exception>> Resolvers =
			new Dictionary<string, Func<string, Gs2Exception, Gs2Exception>>
		{
			{ "Gs2Account", global::Gs2.Gs2Account.Gs2AccountErrorResolver.Resolve },
			{ "Gs2AdReward", global::Gs2.Gs2AdReward.Gs2AdRewardErrorResolver.Resolve },
			{ "Gs2Auth", global::Gs2.Gs2Auth.Gs2AuthErrorResolver.Resolve },
			{ "Gs2Buff", global::Gs2.Gs2Buff.Gs2BuffErrorResolver.Resolve },
			{ "Gs2Chat", global::Gs2.Gs2Chat.Gs2ChatErrorResolver.Resolve },
			{ "Gs2Datastore", global::Gs2.Gs2Datastore.Gs2DatastoreErrorResolver.Resolve },
			{ "Gs2Deploy", global::Gs2.Gs2Deploy.Gs2DeployErrorResolver.Resolve },
			{ "Gs2Dictionary", global::Gs2.Gs2Dictionary.Gs2DictionaryErrorResolver.Resolve },
			{ "Gs2Distributor", global::Gs2.Gs2Distributor.Gs2DistributorErrorResolver.Resolve },
			{ "Gs2Enchant", global::Gs2.Gs2Enchant.Gs2EnchantErrorResolver.Resolve },
			{ "Gs2Enhance", global::Gs2.Gs2Enhance.Gs2EnhanceErrorResolver.Resolve },
			{ "Gs2Exchange", global::Gs2.Gs2Exchange.Gs2ExchangeErrorResolver.Resolve },
			{ "Gs2Experience", global::Gs2.Gs2Experience.Gs2ExperienceErrorResolver.Resolve },
			{ "Gs2Formation", global::Gs2.Gs2Formation.Gs2FormationErrorResolver.Resolve },
			{ "Gs2Freeze", global::Gs2.Gs2Freeze.Gs2FreezeErrorResolver.Resolve },
			{ "Gs2Friend", global::Gs2.Gs2Friend.Gs2FriendErrorResolver.Resolve },
			{ "Gs2Gateway", global::Gs2.Gs2Gateway.Gs2GatewayErrorResolver.Resolve },
			{ "Gs2Grade", global::Gs2.Gs2Grade.Gs2GradeErrorResolver.Resolve },
			{ "Gs2Guard", global::Gs2.Gs2Guard.Gs2GuardErrorResolver.Resolve },
			{ "Gs2Guild", global::Gs2.Gs2Guild.Gs2GuildErrorResolver.Resolve },
			{ "Gs2Identifier", global::Gs2.Gs2Identifier.Gs2IdentifierErrorResolver.Resolve },
			{ "Gs2Idle", global::Gs2.Gs2Idle.Gs2IdleErrorResolver.Resolve },
			{ "Gs2Inbox", global::Gs2.Gs2Inbox.Gs2InboxErrorResolver.Resolve },
			{ "Gs2Inventory", global::Gs2.Gs2Inventory.Gs2InventoryErrorResolver.Resolve },
			{ "Gs2JobQueue", global::Gs2.Gs2JobQueue.Gs2JobQueueErrorResolver.Resolve },
			{ "Gs2Key", global::Gs2.Gs2Key.Gs2KeyErrorResolver.Resolve },
			{ "Gs2Limit", global::Gs2.Gs2Limit.Gs2LimitErrorResolver.Resolve },
			{ "Gs2Lock", global::Gs2.Gs2Lock.Gs2LockErrorResolver.Resolve },
			{ "Gs2Log", global::Gs2.Gs2Log.Gs2LogErrorResolver.Resolve },
			{ "Gs2LoginReward", global::Gs2.Gs2LoginReward.Gs2LoginRewardErrorResolver.Resolve },
			{ "Gs2Lottery", global::Gs2.Gs2Lottery.Gs2LotteryErrorResolver.Resolve },
			{ "Gs2Matchmaking", global::Gs2.Gs2Matchmaking.Gs2MatchmakingErrorResolver.Resolve },
			{ "Gs2MegaField", global::Gs2.Gs2MegaField.Gs2MegaFieldErrorResolver.Resolve },
			{ "Gs2Mission", global::Gs2.Gs2Mission.Gs2MissionErrorResolver.Resolve },
			{ "Gs2Money", global::Gs2.Gs2Money.Gs2MoneyErrorResolver.Resolve },
			{ "Gs2Money2", global::Gs2.Gs2Money2.Gs2Money2ErrorResolver.Resolve },
			{ "Gs2News", global::Gs2.Gs2News.Gs2NewsErrorResolver.Resolve },
			{ "Gs2Project", global::Gs2.Gs2Project.Gs2ProjectErrorResolver.Resolve },
			{ "Gs2Quest", global::Gs2.Gs2Quest.Gs2QuestErrorResolver.Resolve },
			{ "Gs2Ranking", global::Gs2.Gs2Ranking.Gs2RankingErrorResolver.Resolve },
			{ "Gs2Ranking2", global::Gs2.Gs2Ranking2.Gs2Ranking2ErrorResolver.Resolve },
			{ "Gs2Realtime", global::Gs2.Gs2Realtime.Gs2RealtimeErrorResolver.Resolve },
			{ "Gs2Schedule", global::Gs2.Gs2Schedule.Gs2ScheduleErrorResolver.Resolve },
			{ "Gs2Script", global::Gs2.Gs2Script.Gs2ScriptErrorResolver.Resolve },
			{ "Gs2SeasonRating", global::Gs2.Gs2SeasonRating.Gs2SeasonRatingErrorResolver.Resolve },
			{ "Gs2SerialKey", global::Gs2.Gs2SerialKey.Gs2SerialKeyErrorResolver.Resolve },
			{ "Gs2Showcase", global::Gs2.Gs2Showcase.Gs2ShowcaseErrorResolver.Resolve },
			{ "Gs2SkillTree", global::Gs2.Gs2SkillTree.Gs2SkillTreeErrorResolver.Resolve },
			{ "Gs2Stamina", global::Gs2.Gs2Stamina.Gs2StaminaErrorResolver.Resolve },
			{ "Gs2StateMachine", global::Gs2.Gs2StateMachine.Gs2StateMachineErrorResolver.Resolve },
			{ "Gs2Version", global::Gs2.Gs2Version.Gs2VersionErrorResolver.Resolve },
		};

		public static IReadOnlyDictionary<string, Func<string, Gs2Exception, Gs2Exception>> ServiceResolvers => Resolvers;

		public static Gs2Exception Resolve(string service, string method, Gs2Exception error)
		{
			if (error == null) {
				return null;
			}
			if (service == null || !Resolvers.TryGetValue(service, out var resolver)) {
				return error;
			}
			return resolver(method, error);
		}
	}
}
