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

#pragma warning disable CS0618 // Obsolete with a message

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Gs2.Core.Model;
using Gs2.Util.LitJson;
#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Gs2Money2.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public partial class PlatformSetting : IComparable
	{
        public Gs2.Gs2Money2.Model.AppleAppStoreSetting AppleAppStore { set; get; }
        public Gs2.Gs2Money2.Model.GooglePlaySetting GooglePlay { set; get; }
        public Gs2.Gs2Money2.Model.FakeSetting Fake { set; get; }
        public PlatformSetting WithAppleAppStore(Gs2.Gs2Money2.Model.AppleAppStoreSetting appleAppStore) {
            this.AppleAppStore = appleAppStore;
            return this;
        }
        public PlatformSetting WithGooglePlay(Gs2.Gs2Money2.Model.GooglePlaySetting googlePlay) {
            this.GooglePlay = googlePlay;
            return this;
        }
        public PlatformSetting WithFake(Gs2.Gs2Money2.Model.FakeSetting fake) {
            this.Fake = fake;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static PlatformSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            if (data.IsString) {
                data = JsonMapper.ToObject(data.ToString());
            }
            return new PlatformSetting()
                .WithAppleAppStore(!data.Keys.Contains("appleAppStore") || data["appleAppStore"] == null ? null : Gs2.Gs2Money2.Model.AppleAppStoreSetting.FromJson(data["appleAppStore"]))
                .WithGooglePlay(!data.Keys.Contains("googlePlay") || data["googlePlay"] == null ? null : Gs2.Gs2Money2.Model.GooglePlaySetting.FromJson(data["googlePlay"]))
                .WithFake(!data.Keys.Contains("fake") || data["fake"] == null ? null : Gs2.Gs2Money2.Model.FakeSetting.FromJson(data["fake"]));
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["appleAppStore"] = AppleAppStore?.ToJson(),
                ["googlePlay"] = GooglePlay?.ToJson(),
                ["fake"] = Fake?.ToJson(),
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (AppleAppStore != null) {
                writer.WritePropertyName("appleAppStore");
                AppleAppStore.WriteJson(writer);
            }
            if (GooglePlay != null) {
                writer.WritePropertyName("googlePlay");
                GooglePlay.WriteJson(writer);
            }
            if (Fake != null) {
                writer.WritePropertyName("fake");
                Fake.WriteJson(writer);
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as PlatformSetting;
            if (ReferenceEquals(other, null))
            {
                if (ReferenceEquals(obj, null))
                {
                    return 1;
                }
                throw new ArgumentException("Object must be of type PlatformSetting.", nameof(obj));
            }
            var diff = 0;
            diff = ModelComparer.Compare(AppleAppStore, other.AppleAppStore);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(GooglePlay, other.GooglePlay);
            if (diff != 0)
            {
                return diff;
            }
            diff = ModelComparer.Compare(Fake, other.Fake);
            if (diff != 0)
            {
                return diff;
            }
            return 0;
        }

        public void Validate() {
            {
            }
            {
            }
            {
            }
        }

        public object Clone() {
            return new PlatformSetting {
                AppleAppStore = AppleAppStore?.Clone() as Gs2.Gs2Money2.Model.AppleAppStoreSetting,
                GooglePlay = GooglePlay?.Clone() as Gs2.Gs2Money2.Model.GooglePlaySetting,
                Fake = Fake?.Clone() as Gs2.Gs2Money2.Model.FakeSetting,
            };
        }
    }
}