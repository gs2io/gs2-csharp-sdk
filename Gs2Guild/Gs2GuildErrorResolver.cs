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
using System.Linq;
using Gs2.Core.Exception;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Guild
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public static class Gs2GuildErrorResolver
	{
		public static Gs2Exception Resolve(string method, Gs2Exception error)
		{
			if (error?.Errors == null) {
				return error;
			}
			Gs2Exception resolved = null;
			switch (method) {
				case "CreateGuild":
				case "CreateGuildByUserId":
					if (error.Errors.Any(v => v != null && v.code == "user.joinedGuild.tooMany")) {
						resolved = new Exception.MaximumJoinedGuildsReachedException(error);
					}
					break;
				case "DeleteMember":
				case "DeleteMemberByGuildName":
				case "Withdrawal":
				case "WithdrawalByUserId":
					if (error.Errors.Any(v => v != null && v.code == "guild.member.master.require")) {
						resolved = new Exception.GuildMasterRequiredException(error);
					}
					break;
				case "Assume":
				case "AssumeByUserId":
					if (error.Errors.Any(v => v != null && v.code == "guild.member.notFound")) {
						resolved = new Exception.NotIncludedGuildMemberException(error);
					}
					break;
				case "AcceptRequest":
				case "AcceptRequestByGuildName":
					if (error.Errors.Any(v => v != null && v.code == "user.joinedGuild.tooMany")) {
						resolved = new Exception.MaximumJoinedGuildsReachedException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "guild.members.tooMany")) {
						resolved = new Exception.MaximumMembersReachedException(error);
					}
					break;
				case "SendRequest":
				case "SendRequestByUserId":
					if (error.Errors.Any(v => v != null && v.code == "guild.members.tooMany")) {
						resolved = new Exception.MaximumMembersReachedException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "user.joinedGuild.tooMany")) {
						resolved = new Exception.MaximumJoinedGuildsReachedException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "guild.receiveRequests.tooMany")) {
						resolved = new Exception.MaximumReceiveRequestsReachedException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "guild.sendRequests.tooMany")) {
						resolved = new Exception.MaximumSendRequestsReachedException(error);
					}
					else if (error.Errors.Any(v => v != null && v.code == "guild.sendRequests.notMeetJoinRequirements")) {
						resolved = new Exception.DotMeetJoinRequirementsException(error);
					}
					break;
			}
			if (resolved == null) {
				return error;
			}
			resolved.Metadata = error.Metadata;
			return resolved;
		}
	}
}