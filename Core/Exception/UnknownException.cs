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

using System.Collections.Generic;
using Gs2.Core.Model;

namespace Gs2.Core.Exception
{
	public class UnknownException : Gs2Exception
	{
		// ReSharper disable once InconsistentNaming
		public UnknownException(string message) : base(message)
		{

		}
		
		// ReSharper disable once InconsistentNaming
		public UnknownException(RequestError[] errors) : base(errors: errors)
		{
			
		}

		public override bool RecommendRetry => true;
		public override bool RecommendAutoRetry => false;

		public override int StatusCode => 0;
	}
}

