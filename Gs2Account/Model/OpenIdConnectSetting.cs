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
	public partial class OpenIdConnectSetting : IComparable
	{
        public string ConfigurationPath { set; get; }
        public string ClientId { set; get; }
        public string ClientSecret { set; get; }
        public string AppleTeamId { set; get; }
        public string AppleKeyId { set; get; }
        public string ApplePrivateKeyPem { set; get; }
        public string DoneEndpointUrl { set; get; }
        public Gs2.Gs2Account.Model.ScopeValue[] AdditionalScopeValues { set; get; }
        public string[] AdditionalReturnValues { set; get; }
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
        public OpenIdConnectSetting WithAdditionalScopeValues(Gs2.Gs2Account.Model.ScopeValue[] additionalScopeValues) {
            this.AdditionalScopeValues = additionalScopeValues;
            return this;
        }
        public OpenIdConnectSetting WithAdditionalReturnValues(string[] additionalReturnValues) {
            this.AdditionalReturnValues = additionalReturnValues;
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
                .WithDoneEndpointUrl(!data.Keys.Contains("doneEndpointUrl") || data["doneEndpointUrl"] == null ? null : data["doneEndpointUrl"].ToString())
                .WithAdditionalScopeValues(!data.Keys.Contains("additionalScopeValues") || data["additionalScopeValues"] == null || !data["additionalScopeValues"].IsArray ? null : data["additionalScopeValues"].Cast<JsonData>().Select(v => {
                    return Gs2.Gs2Account.Model.ScopeValue.FromJson(v);
                }).ToArray())
                .WithAdditionalReturnValues(!data.Keys.Contains("additionalReturnValues") || data["additionalReturnValues"] == null || !data["additionalReturnValues"].IsArray ? null : data["additionalReturnValues"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray());
        }

        public JsonData ToJson()
        {
            JsonData additionalScopeValuesJsonData = null;
            if (AdditionalScopeValues != null && AdditionalScopeValues.Length > 0)
            {
                additionalScopeValuesJsonData = new JsonData();
                foreach (var additionalScopeValue in AdditionalScopeValues)
                {
                    additionalScopeValuesJsonData.Add(additionalScopeValue.ToJson());
                }
            }
            JsonData additionalReturnValuesJsonData = null;
            if (AdditionalReturnValues != null && AdditionalReturnValues.Length > 0)
            {
                additionalReturnValuesJsonData = new JsonData();
                foreach (var additionalReturnValue in AdditionalReturnValues)
                {
                    additionalReturnValuesJsonData.Add(additionalReturnValue);
                }
            }
            return new JsonData {
                ["configurationPath"] = ConfigurationPath,
                ["clientId"] = ClientId,
                ["clientSecret"] = ClientSecret,
                ["appleTeamId"] = AppleTeamId,
                ["appleKeyId"] = AppleKeyId,
                ["applePrivateKeyPem"] = ApplePrivateKeyPem,
                ["doneEndpointUrl"] = DoneEndpointUrl,
                ["additionalScopeValues"] = additionalScopeValuesJsonData,
                ["additionalReturnValues"] = additionalReturnValuesJsonData,
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
            if (AdditionalScopeValues != null) {
                writer.WritePropertyName("additionalScopeValues");
                writer.WriteArrayStart();
                foreach (var additionalScopeValue in AdditionalScopeValues)
                {
                    if (additionalScopeValue != null) {
                        additionalScopeValue.WriteJson(writer);
                    }
                }
                writer.WriteArrayEnd();
            }
            if (AdditionalReturnValues != null) {
                writer.WritePropertyName("additionalReturnValues");
                writer.WriteArrayStart();
                foreach (var additionalReturnValue in AdditionalReturnValues)
                {
                    if (additionalReturnValue != null) {
                        writer.Write(additionalReturnValue.ToString());
                    }
                }
                writer.WriteArrayEnd();
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
            if (AdditionalScopeValues == null && AdditionalScopeValues == other.AdditionalScopeValues)
            {
                // null and null
            }
            else
            {
                diff += AdditionalScopeValues.Length - other.AdditionalScopeValues.Length;
                for (var i = 0; i < AdditionalScopeValues.Length; i++)
                {
                    diff += AdditionalScopeValues[i].CompareTo(other.AdditionalScopeValues[i]);
                }
            }
            if (AdditionalReturnValues == null && AdditionalReturnValues == other.AdditionalReturnValues)
            {
                // null and null
            }
            else
            {
                diff += AdditionalReturnValues.Length - other.AdditionalReturnValues.Length;
                for (var i = 0; i < AdditionalReturnValues.Length; i++)
                {
                    diff += AdditionalReturnValues[i].CompareTo(other.AdditionalReturnValues[i]);
                }
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
            {
                if (AdditionalScopeValues.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.additionalScopeValues.error.tooMany"),
                    });
                }
            }
            {
                if (AdditionalReturnValues.Length > 10) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("openIdConnectSetting", "account.openIdConnectSetting.additionalReturnValues.error.tooMany"),
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
                AdditionalScopeValues = AdditionalScopeValues?.Clone() as Gs2.Gs2Account.Model.ScopeValue[],
                AdditionalReturnValues = AdditionalReturnValues?.Clone() as string[],
            };
        }
    }
}