﻿/*
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

using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Core.Model
{
	[Preserve]
	public class GeneralError
	{
		// ReSharper disable once InconsistentNaming
		public string message;

		public string Message
		{
			set { message = value; }
			get { return message; }
		}

		public static GeneralError FromDict(JsonData data)
		{
			return new GeneralError
			{
				message = data.Keys.Contains("message") ? (string)data["message"] : null,
			};
		}
	}
}