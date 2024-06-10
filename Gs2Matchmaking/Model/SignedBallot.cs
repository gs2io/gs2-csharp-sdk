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
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Matchmaking.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class SignedBallot : IComparable
	{
        public string Body { set; get; } = null!;
        public string Signature { set; get; } = null!;
        public SignedBallot WithBody(string body) {
            this.Body = body;
            return this;
        }
        public SignedBallot WithSignature(string signature) {
            this.Signature = signature;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static SignedBallot FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new SignedBallot()
                .WithBody(!data.Keys.Contains("body") || data["body"] == null ? null : data["body"].ToString())
                .WithSignature(!data.Keys.Contains("signature") || data["signature"] == null ? null : data["signature"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["body"] = Body,
                ["signature"] = Signature,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (Body != null) {
                writer.WritePropertyName("body");
                writer.Write(Body.ToString());
            }
            if (Signature != null) {
                writer.WritePropertyName("signature");
                writer.Write(Signature.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as SignedBallot;
            var diff = 0;
            if (Body == null && Body == other.Body)
            {
                // null and null
            }
            else
            {
                diff += Body.CompareTo(other.Body);
            }
            if (Signature == null && Signature == other.Signature)
            {
                // null and null
            }
            else
            {
                diff += Signature.CompareTo(other.Signature);
            }
            return diff;
        }

        public void Validate() {
            {
                if (Body.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("signedBallot", "matchmaking.signedBallot.body.error.tooLong"),
                    });
                }
            }
            {
                if (Signature.Length > 256) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("signedBallot", "matchmaking.signedBallot.signature.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new SignedBallot {
                Body = Body,
                Signature = Signature,
            };
        }
    }
}