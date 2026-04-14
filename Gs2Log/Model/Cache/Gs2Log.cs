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

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using Gs2.Core.Domain;

namespace Gs2.Gs2Log.Model.Cache
{
    public static class Gs2Log
    {
        public static void PutCache(
            CacheDatabase cache,
            string userId,
            string method, 
            string requestPayload, 
            string resultPayload
        ) {
            switch (method) {
                case "describeNamespaces":
                    Result.DescribeNamespacesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeNamespacesRequest.FromJson(requestPayload)
                    );
                    break;
                case "createNamespace":
                    Result.CreateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespaceStatus":
                    Result.GetNamespaceStatusResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceStatusRequest.FromJson(requestPayload)
                    );
                    break;
                case "getNamespace":
                    Result.GetNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateNamespace":
                    Result.UpdateNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteNamespace":
                    Result.DeleteNamespaceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteNamespaceRequest.FromJson(requestPayload)
                    );
                    break;
                case "getServiceVersion":
                    Result.GetServiceVersionResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetServiceVersionRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryAccessLog":
                    Result.QueryAccessLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryAccessLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "countAccessLog":
                    Result.CountAccessLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountAccessLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryIssueStampSheetLog":
                    Result.QueryIssueStampSheetLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryIssueStampSheetLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "countIssueStampSheetLog":
                    Result.CountIssueStampSheetLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountIssueStampSheetLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryExecuteStampSheetLog":
                    Result.QueryExecuteStampSheetLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryExecuteStampSheetLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "countExecuteStampSheetLog":
                    Result.CountExecuteStampSheetLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountExecuteStampSheetLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryExecuteStampTaskLog":
                    Result.QueryExecuteStampTaskLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryExecuteStampTaskLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "countExecuteStampTaskLog":
                    Result.CountExecuteStampTaskLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CountExecuteStampTaskLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryInGameLog":
                    Result.QueryInGameLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryInGameLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendInGameLog":
                    Result.SendInGameLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SendInGameLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "sendInGameLogByUserId":
                    Result.SendInGameLogByUserIdResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.SendInGameLogByUserIdRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryAccessLogWithTelemetry":
                    Result.QueryAccessLogWithTelemetryResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryAccessLogWithTelemetryRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeInsights":
                    Result.DescribeInsightsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeInsightsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createInsight":
                    Result.CreateInsightResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateInsightRequest.FromJson(requestPayload)
                    );
                    break;
                case "getInsight":
                    Result.GetInsightResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetInsightRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteInsight":
                    Result.DeleteInsightResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteInsightRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeFacetModels":
                    Result.DescribeFacetModelsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeFacetModelsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createFacetModel":
                    Result.CreateFacetModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateFacetModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "getFacetModel":
                    Result.GetFacetModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetFacetModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateFacetModel":
                    Result.UpdateFacetModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateFacetModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteFacetModel":
                    Result.DeleteFacetModelResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteFacetModelRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeDashboards":
                    Result.DescribeDashboardsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeDashboardsRequest.FromJson(requestPayload)
                    );
                    break;
                case "createDashboard":
                    Result.CreateDashboardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.CreateDashboardRequest.FromJson(requestPayload)
                    );
                    break;
                case "getDashboard":
                    Result.GetDashboardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetDashboardRequest.FromJson(requestPayload)
                    );
                    break;
                case "updateDashboard":
                    Result.UpdateDashboardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.UpdateDashboardRequest.FromJson(requestPayload)
                    );
                    break;
                case "duplicateDashboard":
                    Result.DuplicateDashboardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DuplicateDashboardRequest.FromJson(requestPayload)
                    );
                    break;
                case "deleteDashboard":
                    Result.DeleteDashboardResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DeleteDashboardRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryLog":
                    Result.QueryLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "getLog":
                    Result.GetLogResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetLogRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryFacets":
                    Result.QueryFacetsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryFacetsRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryTimeseries":
                    Result.QueryTimeseriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryTimeseriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "getTrace":
                    Result.GetTraceResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.GetTraceRequest.FromJson(requestPayload)
                    );
                    break;
                case "queryMetricsTimeseries":
                    Result.QueryMetricsTimeseriesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.QueryMetricsTimeseriesRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeMetrics":
                    Result.DescribeMetricsResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeMetricsRequest.FromJson(requestPayload)
                    );
                    break;
                case "describeLabelValues":
                    Result.DescribeLabelValuesResult.FromJson(resultPayload).PutCache(
                        cache,
                        userId,
                        null,
                        Request.DescribeLabelValuesRequest.FromJson(requestPayload)
                    );
                    break;
            }
        }
    }
}