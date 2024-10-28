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

namespace Gs2.Gs2Account.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class OpenIdConnectSetting : IComparable
	{
        public string ConfigurationPath { set; get; } = null!;
        public string ClientId { set; get; } = null!;
        public string ClientSecret { set; get; } = null!;
        public string AppleTeamId { set; get; } = null!;
        public string AppleKeyId { set; get; } = null!;
        public string ApplePrivateKeyPem { set; get; } = null!;
        public string DoneEndpointUrl { set; get; } = null!;
        public OpenIdConnectSetting WithConfigurationPath(string configurationPath) {
            this.ConfigurationPath = configurationPath;
            return this;
        }
        public OpenIdConnectSetting WithClientId(string clientId) {
            this.ClientId = clientId;
            return this;
        }
        public OpenIdConnectSetting WithClientSecret(string clientSecret) {
            this.ClientSecret = clientSecret;
            return this;
        }
        public OpenIdConnectSetting WithAppleTeamId(string appleTeamId) {
            this.AppleTeamId = appleTeamId;
            return this;
        }
        public OpenIdConnectSetting WithAppleKeyId(string appleKeyId) {
            this.AppleKeyId = appleKeyId;
            return this;
        }
        public OpenIdConnectSetting WithApplePrivateKeyPem(string applePrivateKeyPem) {
            this.ApplePrivateKeyPem = applePrivateKeyPem;
            return this;
        }
        public OpenIdConnectSetting WithDoneEndpointUrl(string doneEndpointUrl) {
            this.DoneEndpointUrl = doneEndpointUrl;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static OpenIdConnectSetting FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new OpenIdConnectSetting()
                .WithConfigurationPath(!data.Keys.Contains("configurationPath") || data["configurationPath"] == null ? null : data["configurationPath"].ToString())
                .WithClientId(!data.Keys.Contains("clientId") || data["clientId"] == null ? null : data["clientId"].ToString())
                .WithClientSecret(!data.Keys.Contains("clientSecret") || data["clientSecret"] == null ? null : data["clientSecret"].ToString())
                .WithAppleTeamId(!data.Keys.Contains("appleTeamId") || data["appleTeamId"] == null ? null : data["appleTeamId"].ToString())
                .WithAppleKeyId(!data.Keys.Contains("appleKeyId") || data["appleKeyId"] == null ? null : data["appleKeyId"].ToString())
                .WithApplePrivateKeyPem(!data.Keys.Contains("applePrivateKeyPem") || data["applePrivateKeyPem"] == null ? null : data["applePrivateKeyPem"].ToString())
                .WithDoneEndpointUrl(!data.Keys.Contains("doneEndpointUrl") || data["doneEndpointUrl"] == null ? null : data["doneEndpointUrl"].ToString());
        }

        public JsonData ToJson()
        {
            return new JsonData {
                ["configurationPath"] = ConfigurationPath,
                ["clientId"] = ClientId,
                ["clientSecret"] = ClientSecret,
                ["appleTeamId"] = AppleTeamId,
                ["appleKeyId"] = AppleKeyId,
                ["applePrivateKeyPem"] = ApplePrivateKeyPem,
                ["doneEndpointUrl"] = DoneEndpointUrl,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (ConfigurationPath != null) {
                writer.WritePropertyName("configurationPath");
                writer.Write(ConfigurationPath.ToString());
            }
            if (ClientId != null) {
                writer.WritePropertyName("clientId");
                writer.Write(ClientId.ToString());
            }
            if (ClientSecret != null) {
                writer.WritePropertyName("clientSecret");
                writer.Write(ClientSecret.ToString());
            }
            if (AppleTeamId != null) {
                writer.WritePropertyName("appleTeamId");
                writer.Write(AppleTeamId.ToString());
            }
            if (AppleKeyId != null) {
                writer.WritePropertyName("appleKeyId");
                writer.Write(AppleKeyId.ToString());
            }
            if (ApplePrivateKeyPem != null) {
                writer.WritePropertyName("applePrivateKeyPem");
                writer.Write(ApplePrivateKeyPem.ToString());
            }
            if (DoneEndpointUrl != null) {
                writer.WritePropertyName("doneEndpointUrl");
                writer.Write(DoneEndpointUrl.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as OpenIdConnectSetting;
            var diff = 0;
            if (ConfigurationPath == null && ConfigurationPath == other.ConfigurationPath)
            {
                // null and null
            }
            else
            {
                diff += ConfigurationPath.CompareTo(other.ConfigurationPath);
            }
            if (ClientId == null && ClientId == other.ClientId)
            {
                // null and null
            }
            else
            {
                diff += ClientId.CompareTo(other.ClientId);
            }
            if (ClientSecret == null && ClientSecret == other.ClientSecret)
            {
                // null and null
            }
            else
            {
                diff += ClientSecret.CompareTo(other.ClientSecret);
            }
            if (AppleTeamId == null && AppleTeamId == other.AppleTeamId)
            {
                // null and null
            }
            else
            {
                diff += AppleTeamId.CompareTo(other.AppleTeamId);
            }
            if (AppleKeyId == null && AppleKeyId == other.AppleKeyId)
            {
                // null and null
            }
            else
            {
                diff += AppleKeyId.CompareTo(other.AppleKeyId);
            }
            if (ApplePrivateKeyPem == null && ApplePrivateKeyPem == other.ApplePrivateKeyPem)
            {
                // null and null
            }
            else
            {
                diff += ApplePrivateKeyPem.CompareTo(other.ApplePrivateKeyPem);
            }
            if (DoneEndpointUrl == null && DoneEndpointUrl == other.DoneEndpointUrl)
            {
                // null and null
            }
            else
            {
                diff += DoneEndpointUrl.CompareTo(other.DoneEndpointUrl);
            }
            return diff;
        }

        public void Validate() {
            {
                if (ConfigurationPath.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.configurationPath.error.tooLong"),
                    });
                }
            }
            {
                if (ClientId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.clientId.error.tooLong"),
                    });
                }
            }
            if (ConfigurationPath != "https://appleid.apple.com/.well-known/openid-configuration") {
                if (ClientSecret.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.clientSecret.error.tooLong"),
                    });
                }
            }
            if (ConfigurationPath == "https://appleid.apple.com/.well-known/openid-configuration") {
                if (AppleTeamId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.appleTeamId.error.tooLong"),
                    });
                }
            }
            if (ConfigurationPath == "https://appleid.apple.com/.well-known/openid-configuration") {
                if (AppleKeyId.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.appleKeyId.error.tooLong"),
                    });
                }
            }
            if (ConfigurationPath == "https://appleid.apple.com/.well-known/openid-configuration") {
                if (ApplePrivateKeyPem.Length > 10240) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.applePrivateKeyPem.error.tooLong"),
                    });
                }
            }
            {
                if (DoneEndpointUrl.Length > 1024) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.doneEndpointUrl.error.tooLong"),
                    });
                }
            }
        }

        public object Clone() {
            return new OpenIdConnectSetting {
                ConfigurationPath = ConfigurationPath,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                AppleTeamId = AppleTeamId,
                AppleKeyId = AppleKeyId,
                ApplePrivateKeyPem = ApplePrivateKeyPem,
                DoneEndpointUrl = DoneEndpointUrl,
            };
        }
    }
}