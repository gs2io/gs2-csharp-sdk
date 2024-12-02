using System;
using System.Collections;
using System.Threading.Tasks;
using Gs2.Core.Exception;
using Gs2.Core.Model;
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
            string stampSheetEncryptionKeyId,
            bool? atomicCommit,
            TransactionResult transactionResult
        ) {
            if (autoRun) {
                if (atomicCommit ?? false) {
                    if (transactionResult != null) {
                        return new RanTransactionAccessTokenDomain(
                            gs2,
                            transactionId,
                            accessToken,
                            transactionResult
                        );
                    }
                    return new AutoTransactionAccessTokenDomain(
                        gs2,
                        accessToken,
                        transactionId
                    );
                }
                return new AutoStampSheetAccessTokenDomain(
                    gs2,
                    accessToken,
                    transactionId
                );
            }
            else {
                return new ManualStampSheetAccessTokenDomain(
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
            string stampSheetEncryptionKeyId,
            bool? atomicCommit,
            TransactionResult transactionResult
        ) {
            if (autoRun) {
                if (atomicCommit ?? false) {
                    if (transactionResult != null) {
                        return new RanTransactionDomain(
                            gs2,
                            transactionId,
                            userId,
                            transactionResult
                        );
                    }
                    return new AutoTransactionDomain(
                        gs2,
                        userId,
                        transactionId
                    );
                }
                return new AutoStampSheetDomain(
                    gs2,
                    userId,
                    transactionId
                );
            }
            else {
                return new ManualStampSheetDomain(
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