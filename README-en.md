[⇒日本語のREADMEへ](README.md)

# GS2-CSharp-SDK

SDK for Game Server Services(https://gs2.io) in C#.

It can be used in combination with the .NET Core runtime.

[GS2 SDK for Unity](https://github.com/gs2io/gs2-sdk-for-unity) consists of GS2-CSharp-SDK (for C# environment) and GS2 SDK for Unity (for Unity environment).

## What is Game Server Services?

Game Server Services(GS2) is a back-end server service (BaaS) specialized for game development.

GS2 is a general-purpose game server solution created to improve efficiency for game developers and supports Games as a Service (GaaS) and Live Gaming.

The service allows for flexible management of player data and data analysis, enabling proper analysis of in-game resource distribution and consumption to maintain a healthy environment.
In addition, it provides story progression management and possession management, contributing to game monetization and player engagement.
GS2 supports online functionality and makes it easy for game developers to analyze data and manage economics to help their games succeed.

## Getting Started

Place the downloaded source code in the following folder in your Unity project.

`(Unity Project)/Assets/Scripts/Runtime/Sdk/Gs2`

GS2 credentials are required to use the SDK.
Follow the instructions in [GS2 Setup](https://docs.gs2.io/en/get_start/tutorial/setup_gs2/) to issue the credential.

## Retrieving with NuGet

You can get the package from NuGet.

GS2.CSharp.Sdk
https://www.nuget.org/packages/GS2.CSharp.Sdk

### Requirements

- C# 8.0 or higher

[⇒Start using GS2 - SDK - Various programming languages](https://docs.gs2.io/en/get_start/#various-programming-languages)

## SDK detailed specifications

For details on the API for each service and communication method, please refer to the

 [⇒API Reference](https://docs.gs2.io/en/api_reference/) page.

For information on the initialization process, please refer to the

 [⇒API Reference - Initialization process](https://docs.gs2.io/en/api_reference/initialize/) page.

*All code in this project is auto-generated except for Core, so we cannot respond to individual Pull-Requests. *
