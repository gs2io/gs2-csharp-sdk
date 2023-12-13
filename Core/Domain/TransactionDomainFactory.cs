using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Net;
using Gs2.Gs2Auth.Model;
using Gs2.Gs2Distributor;
using Gs2.Gs2Distributor.Request;
using Gs2.Gs2Distributor.Result;
using Gs2.Gs2JobQueue.Result;
using Gs2.Util.LitJson;
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace Gs2.Core.Domain
{
    public static class TransactionDomainFactory
    {
        public static TransactionAccessTokenDomain ToTransaction(
            Gs2 gs2,
            AccessToken accessToken,
            bool autoRun,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKeyId
        ) {
            if (autoRun) {
                return new AutoTransactionAccessTokenDomain(
                    gs2,
                    accessToken,
                    transactionId
                );
            }
            else {
                return new ManualTransactionAccessTokenDomain(
                    gs2,
                    accessToken,
                    transactionId,
                    stampSheet,
                    stampSheetEncryptionKeyId
                );
            }
        }
        
        public static TransactionDomain ToTransaction(
            Gs2 gs2,
            string userId,
            bool autoRun,
            string transactionId,
            string stampSheet,
            string stampSheetEncryptionKeyId
        ) {
            if (autoRun) {
                return new AutoTransactionDomain(
                    gs2,
                    userId,
                    transactionId
                );
            }
            else {
                return new ManualTransactionDomain(
                    gs2,
                    userId,
                    transactionId,
                    stampSheet,
                    stampSheetEncryptionKeyId
                );
            }
        }
    }
}