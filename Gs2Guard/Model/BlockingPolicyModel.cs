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

namespace Gs2.Gs2Guard.Model
{

#if UNITY_2017_1_OR_NEWER
	[Preserve]
#endif
	public class BlockingPolicyModel : IComparable
	{
        public string[] PassServices { set; get; } = null!;
        public string DefaultRestriction { set; get; } = null!;
        public string LocationDetection { set; get; } = null!;
        public string[] Locations { set; get; } = null!;
        public string LocationRestriction { set; get; } = null!;
        public string AnonymousIpDetection { set; get; } = null!;
        public string AnonymousIpRestriction { set; get; } = null!;
        public string HostingProviderIpDetection { set; get; } = null!;
        public string HostingProviderIpRestriction { set; get; } = null!;
        public string ReputationIpDetection { set; get; } = null!;
        public string ReputationIpRestriction { set; get; } = null!;
        public string IpAddressesDetection { set; get; } = null!;
        public string[] IpAddresses { set; get; } = null!;
        public string IpAddressRestriction { set; get; } = null!;
        public BlockingPolicyModel WithPassServices(string[] passServices) {
            this.PassServices = passServices;
            return this;
        }
        public BlockingPolicyModel WithDefaultRestriction(string defaultRestriction) {
            this.DefaultRestriction = defaultRestriction;
            return this;
        }
        public BlockingPolicyModel WithLocationDetection(string locationDetection) {
            this.LocationDetection = locationDetection;
            return this;
        }
        public BlockingPolicyModel WithLocations(string[] locations) {
            this.Locations = locations;
            return this;
        }
        public BlockingPolicyModel WithLocationRestriction(string locationRestriction) {
            this.LocationRestriction = locationRestriction;
            return this;
        }
        public BlockingPolicyModel WithAnonymousIpDetection(string anonymousIpDetection) {
            this.AnonymousIpDetection = anonymousIpDetection;
            return this;
        }
        public BlockingPolicyModel WithAnonymousIpRestriction(string anonymousIpRestriction) {
            this.AnonymousIpRestriction = anonymousIpRestriction;
            return this;
        }
        public BlockingPolicyModel WithHostingProviderIpDetection(string hostingProviderIpDetection) {
            this.HostingProviderIpDetection = hostingProviderIpDetection;
            return this;
        }
        public BlockingPolicyModel WithHostingProviderIpRestriction(string hostingProviderIpRestriction) {
            this.HostingProviderIpRestriction = hostingProviderIpRestriction;
            return this;
        }
        public BlockingPolicyModel WithReputationIpDetection(string reputationIpDetection) {
            this.ReputationIpDetection = reputationIpDetection;
            return this;
        }
        public BlockingPolicyModel WithReputationIpRestriction(string reputationIpRestriction) {
            this.ReputationIpRestriction = reputationIpRestriction;
            return this;
        }
        public BlockingPolicyModel WithIpAddressesDetection(string ipAddressesDetection) {
            this.IpAddressesDetection = ipAddressesDetection;
            return this;
        }
        public BlockingPolicyModel WithIpAddresses(string[] ipAddresses) {
            this.IpAddresses = ipAddresses;
            return this;
        }
        public BlockingPolicyModel WithIpAddressRestriction(string ipAddressRestriction) {
            this.IpAddressRestriction = ipAddressRestriction;
            return this;
        }

#if UNITY_2017_1_OR_NEWER
    	[Preserve]
#endif
        public static BlockingPolicyModel FromJson(JsonData data)
        {
            if (data == null) {
                return null;
            }
            return new BlockingPolicyModel()
                .WithPassServices(!data.Keys.Contains("passServices") || data["passServices"] == null || !data["passServices"].IsArray ? null : data["passServices"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithDefaultRestriction(!data.Keys.Contains("defaultRestriction") || data["defaultRestriction"] == null ? null : data["defaultRestriction"].ToString())
                .WithLocationDetection(!data.Keys.Contains("locationDetection") || data["locationDetection"] == null ? null : data["locationDetection"].ToString())
                .WithLocations(!data.Keys.Contains("locations") || data["locations"] == null || !data["locations"].IsArray ? null : data["locations"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithLocationRestriction(!data.Keys.Contains("locationRestriction") || data["locationRestriction"] == null ? null : data["locationRestriction"].ToString())
                .WithAnonymousIpDetection(!data.Keys.Contains("anonymousIpDetection") || data["anonymousIpDetection"] == null ? null : data["anonymousIpDetection"].ToString())
                .WithAnonymousIpRestriction(!data.Keys.Contains("anonymousIpRestriction") || data["anonymousIpRestriction"] == null ? null : data["anonymousIpRestriction"].ToString())
                .WithHostingProviderIpDetection(!data.Keys.Contains("hostingProviderIpDetection") || data["hostingProviderIpDetection"] == null ? null : data["hostingProviderIpDetection"].ToString())
                .WithHostingProviderIpRestriction(!data.Keys.Contains("hostingProviderIpRestriction") || data["hostingProviderIpRestriction"] == null ? null : data["hostingProviderIpRestriction"].ToString())
                .WithReputationIpDetection(!data.Keys.Contains("reputationIpDetection") || data["reputationIpDetection"] == null ? null : data["reputationIpDetection"].ToString())
                .WithReputationIpRestriction(!data.Keys.Contains("reputationIpRestriction") || data["reputationIpRestriction"] == null ? null : data["reputationIpRestriction"].ToString())
                .WithIpAddressesDetection(!data.Keys.Contains("ipAddressesDetection") || data["ipAddressesDetection"] == null ? null : data["ipAddressesDetection"].ToString())
                .WithIpAddresses(!data.Keys.Contains("ipAddresses") || data["ipAddresses"] == null || !data["ipAddresses"].IsArray ? null : data["ipAddresses"].Cast<JsonData>().Select(v => {
                    return v.ToString();
                }).ToArray())
                .WithIpAddressRestriction(!data.Keys.Contains("ipAddressRestriction") || data["ipAddressRestriction"] == null ? null : data["ipAddressRestriction"].ToString());
        }

        public JsonData ToJson()
        {
            JsonData passServicesJsonData = null;
            if (PassServices != null && PassServices.Length > 0)
            {
                passServicesJsonData = new JsonData();
                foreach (var passService in PassServices)
                {
                    passServicesJsonData.Add(passService);
                }
            }
            JsonData locationsJsonData = null;
            if (Locations != null && Locations.Length > 0)
            {
                locationsJsonData = new JsonData();
                foreach (var location in Locations)
                {
                    locationsJsonData.Add(location);
                }
            }
            JsonData ipAddressesJsonData = null;
            if (IpAddresses != null && IpAddresses.Length > 0)
            {
                ipAddressesJsonData = new JsonData();
                foreach (var ipAddress in IpAddresses)
                {
                    ipAddressesJsonData.Add(ipAddress);
                }
            }
            return new JsonData {
                ["passServices"] = passServicesJsonData,
                ["defaultRestriction"] = DefaultRestriction,
                ["locationDetection"] = LocationDetection,
                ["locations"] = locationsJsonData,
                ["locationRestriction"] = LocationRestriction,
                ["anonymousIpDetection"] = AnonymousIpDetection,
                ["anonymousIpRestriction"] = AnonymousIpRestriction,
                ["hostingProviderIpDetection"] = HostingProviderIpDetection,
                ["hostingProviderIpRestriction"] = HostingProviderIpRestriction,
                ["reputationIpDetection"] = ReputationIpDetection,
                ["reputationIpRestriction"] = ReputationIpRestriction,
                ["ipAddressesDetection"] = IpAddressesDetection,
                ["ipAddresses"] = ipAddressesJsonData,
                ["ipAddressRestriction"] = IpAddressRestriction,
            };
        }

        public void WriteJson(JsonWriter writer)
        {
            writer.WriteObjectStart();
            if (PassServices != null) {
                writer.WritePropertyName("passServices");
                writer.WriteArrayStart();
                foreach (var passService in PassServices)
                {
                    if (passService != null) {
                        writer.Write(passService.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (DefaultRestriction != null) {
                writer.WritePropertyName("defaultRestriction");
                writer.Write(DefaultRestriction.ToString());
            }
            if (LocationDetection != null) {
                writer.WritePropertyName("locationDetection");
                writer.Write(LocationDetection.ToString());
            }
            if (Locations != null) {
                writer.WritePropertyName("locations");
                writer.WriteArrayStart();
                foreach (var location in Locations)
                {
                    if (location != null) {
                        writer.Write(location.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (LocationRestriction != null) {
                writer.WritePropertyName("locationRestriction");
                writer.Write(LocationRestriction.ToString());
            }
            if (AnonymousIpDetection != null) {
                writer.WritePropertyName("anonymousIpDetection");
                writer.Write(AnonymousIpDetection.ToString());
            }
            if (AnonymousIpRestriction != null) {
                writer.WritePropertyName("anonymousIpRestriction");
                writer.Write(AnonymousIpRestriction.ToString());
            }
            if (HostingProviderIpDetection != null) {
                writer.WritePropertyName("hostingProviderIpDetection");
                writer.Write(HostingProviderIpDetection.ToString());
            }
            if (HostingProviderIpRestriction != null) {
                writer.WritePropertyName("hostingProviderIpRestriction");
                writer.Write(HostingProviderIpRestriction.ToString());
            }
            if (ReputationIpDetection != null) {
                writer.WritePropertyName("reputationIpDetection");
                writer.Write(ReputationIpDetection.ToString());
            }
            if (ReputationIpRestriction != null) {
                writer.WritePropertyName("reputationIpRestriction");
                writer.Write(ReputationIpRestriction.ToString());
            }
            if (IpAddressesDetection != null) {
                writer.WritePropertyName("ipAddressesDetection");
                writer.Write(IpAddressesDetection.ToString());
            }
            if (IpAddresses != null) {
                writer.WritePropertyName("ipAddresses");
                writer.WriteArrayStart();
                foreach (var ipAddress in IpAddresses)
                {
                    if (ipAddress != null) {
                        writer.Write(ipAddress.ToString());
                    }
                }
                writer.WriteArrayEnd();
            }
            if (IpAddressRestriction != null) {
                writer.WritePropertyName("ipAddressRestriction");
                writer.Write(IpAddressRestriction.ToString());
            }
            writer.WriteObjectEnd();
        }

        public int CompareTo(object obj)
        {
            var other = obj as BlockingPolicyModel;
            var diff = 0;
            if (PassServices == null && PassServices == other.PassServices)
            {
                // null and null
            }
            else
            {
                diff += PassServices.Length - other.PassServices.Length;
                for (var i = 0; i < PassServices.Length; i++)
                {
                    diff += PassServices[i].CompareTo(other.PassServices[i]);
                }
            }
            if (DefaultRestriction == null && DefaultRestriction == other.DefaultRestriction)
            {
                // null and null
            }
            else
            {
                diff += DefaultRestriction.CompareTo(other.DefaultRestriction);
            }
            if (LocationDetection == null && LocationDetection == other.LocationDetection)
            {
                // null and null
            }
            else
            {
                diff += LocationDetection.CompareTo(other.LocationDetection);
            }
            if (Locations == null && Locations == other.Locations)
            {
                // null and null
            }
            else
            {
                diff += Locations.Length - other.Locations.Length;
                for (var i = 0; i < Locations.Length; i++)
                {
                    diff += Locations[i].CompareTo(other.Locations[i]);
                }
            }
            if (LocationRestriction == null && LocationRestriction == other.LocationRestriction)
            {
                // null and null
            }
            else
            {
                diff += LocationRestriction.CompareTo(other.LocationRestriction);
            }
            if (AnonymousIpDetection == null && AnonymousIpDetection == other.AnonymousIpDetection)
            {
                // null and null
            }
            else
            {
                diff += AnonymousIpDetection.CompareTo(other.AnonymousIpDetection);
            }
            if (AnonymousIpRestriction == null && AnonymousIpRestriction == other.AnonymousIpRestriction)
            {
                // null and null
            }
            else
            {
                diff += AnonymousIpRestriction.CompareTo(other.AnonymousIpRestriction);
            }
            if (HostingProviderIpDetection == null && HostingProviderIpDetection == other.HostingProviderIpDetection)
            {
                // null and null
            }
            else
            {
                diff += HostingProviderIpDetection.CompareTo(other.HostingProviderIpDetection);
            }
            if (HostingProviderIpRestriction == null && HostingProviderIpRestriction == other.HostingProviderIpRestriction)
            {
                // null and null
            }
            else
            {
                diff += HostingProviderIpRestriction.CompareTo(other.HostingProviderIpRestriction);
            }
            if (ReputationIpDetection == null && ReputationIpDetection == other.ReputationIpDetection)
            {
                // null and null
            }
            else
            {
                diff += ReputationIpDetection.CompareTo(other.ReputationIpDetection);
            }
            if (ReputationIpRestriction == null && ReputationIpRestriction == other.ReputationIpRestriction)
            {
                // null and null
            }
            else
            {
                diff += ReputationIpRestriction.CompareTo(other.ReputationIpRestriction);
            }
            if (IpAddressesDetection == null && IpAddressesDetection == other.IpAddressesDetection)
            {
                // null and null
            }
            else
            {
                diff += IpAddressesDetection.CompareTo(other.IpAddressesDetection);
            }
            if (IpAddresses == null && IpAddresses == other.IpAddresses)
            {
                // null and null
            }
            else
            {
                diff += IpAddresses.Length - other.IpAddresses.Length;
                for (var i = 0; i < IpAddresses.Length; i++)
                {
                    diff += IpAddresses[i].CompareTo(other.IpAddresses[i]);
                }
            }
            if (IpAddressRestriction == null && IpAddressRestriction == other.IpAddressRestriction)
            {
                // null and null
            }
            else
            {
                diff += IpAddressRestriction.CompareTo(other.IpAddressRestriction);
            }
            return diff;
        }

        public void Validate() {
            {
                if (PassServices.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.passServices.error.tooFew"),
                    });
                }
                if (PassServices.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.passServices.error.tooMany"),
                    });
                }
            }
            {
                switch (DefaultRestriction) {
                    case "Allow":
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.defaultRestriction.error.invalid"),
                        });
                }
            }
            {
                switch (LocationDetection) {
                    case "Enable":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.locationDetection.error.invalid"),
                        });
                }
            }
            if (LocationDetection == "Enable") {
                if (Locations.Length < 1) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.locations.error.tooFew"),
                    });
                }
                if (Locations.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.locations.error.tooMany"),
                    });
                }
            }
            if (LocationDetection == "Enable") {
                switch (LocationRestriction) {
                    case "Allow":
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.locationRestriction.error.invalid"),
                        });
                }
            }
            {
                switch (AnonymousIpDetection) {
                    case "Enable":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.anonymousIpDetection.error.invalid"),
                        });
                }
            }
            if (AnonymousIpDetection == "Enable") {
                switch (AnonymousIpRestriction) {
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.anonymousIpRestriction.error.invalid"),
                        });
                }
            }
            {
                switch (HostingProviderIpDetection) {
                    case "Enable":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.hostingProviderIpDetection.error.invalid"),
                        });
                }
            }
            if (HostingProviderIpDetection == "Enable") {
                switch (HostingProviderIpRestriction) {
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.hostingProviderIpRestriction.error.invalid"),
                        });
                }
            }
            {
                switch (ReputationIpDetection) {
                    case "Enable":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.reputationIpDetection.error.invalid"),
                        });
                }
            }
            if (ReputationIpDetection == "Enable") {
                switch (ReputationIpRestriction) {
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.reputationIpRestriction.error.invalid"),
                        });
                }
            }
            {
                switch (IpAddressesDetection) {
                    case "Enable":
                    case "Disable":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.ipAddressesDetection.error.invalid"),
                        });
                }
            }
            {
                if (IpAddresses.Length > 100) {
                    throw new Gs2.Core.Exception.BadRequestException(new [] {
                        new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.ipAddresses.error.tooMany"),
                    });
                }
            }
            if (IpAddressesDetection == "Enable") {
                switch (IpAddressRestriction) {
                    case "Allow":
                    case "Deny":
                        break;
                    default:
                        throw new Gs2.Core.Exception.BadRequestException(new [] {
                            new RequestError("blockingPolicyModel", "guard.blockingPolicyModel.ipAddressRestriction.error.invalid"),
                        });
                }
            }
        }

        public object Clone() {
            return new BlockingPolicyModel {
                PassServices = PassServices.Clone() as string[],
                DefaultRestriction = DefaultRestriction,
                LocationDetection = LocationDetection,
                Locations = Locations.Clone() as string[],
                LocationRestriction = LocationRestriction,
                AnonymousIpDetection = AnonymousIpDetection,
                AnonymousIpRestriction = AnonymousIpRestriction,
                HostingProviderIpDetection = HostingProviderIpDetection,
                HostingProviderIpRestriction = HostingProviderIpRestriction,
                ReputationIpDetection = ReputationIpDetection,
                ReputationIpRestriction = ReputationIpRestriction,
                IpAddressesDetection = IpAddressesDetection,
                IpAddresses = IpAddresses.Clone() as string[],
                IpAddressRestriction = IpAddressRestriction,
            };
        }
    }
}