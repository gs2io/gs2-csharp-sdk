using System;

namespace Gs2.Core.Domain
{
    
    
    public class TransactionConfiguration
    {
        public string NamespaceName { get; set; }
        [Obsolete("Use ConsumeActionEventHandler property instead.")]
        public Action<CacheDatabase, string, int?, string, string, string> StampTaskEventHandler => ConsumeActionEventHandler;
        [Obsolete("Use AcquireActionEventHandler property instead.")]
        public Action<CacheDatabase, string, int?, string, string, string> StampSheetEventHandler => AcquireActionEventHandler;
        public Action<CacheDatabase, string, int?, string, string, string> VerifyActionEventHandler { get; set; }
        public Action<CacheDatabase, string, int?, string, string, string> ConsumeActionEventHandler { get; set; }
        public Action<CacheDatabase, string, int?, string, string, string> AcquireActionEventHandler { get; set; }

        private TransactionConfiguration(
            string namespaceName,
            Action<CacheDatabase, string, int?, string, string, string> verifyActionEventHandler,
            Action<CacheDatabase, string, int?, string, string, string> consumeActionEventHandler,
            Action<CacheDatabase, string, int?, string, string, string> acquireActionEventHandler
        ) {
            this.NamespaceName = namespaceName;
            this.VerifyActionEventHandler = verifyActionEventHandler;
            this.ConsumeActionEventHandler = consumeActionEventHandler;
            this.AcquireActionEventHandler = acquireActionEventHandler;
        }

        public static TransactionConfigurationBuilder Builder() {
            return new TransactionConfigurationBuilder();
        }

        public class TransactionConfigurationBuilder {

            private string NamespaceName { get; set; }
            private Action<CacheDatabase, string, int?, string, string, string> VerifyActionEventHandler { get; set; }
            private Action<CacheDatabase, string, int?, string, string, string> ConsumeActionEventHandler { get; set; }
            private Action<CacheDatabase, string, int?, string, string, string> AcquireActionEventHandler { get; set; }

            internal TransactionConfigurationBuilder(){}

            public TransactionConfigurationBuilder WithNamespaceName(string namespaceName) {
                this.NamespaceName = namespaceName;
                return this;
            }
            [Obsolete("Use WithConsumeActionEventHandler property instead.")]
            public TransactionConfigurationBuilder WithStampTaskEventHandler(Action<CacheDatabase, string, int?, string, string, string> stampTaskEventHandler) {
                this.ConsumeActionEventHandler = stampTaskEventHandler;
                return this;
            }
            [Obsolete("Use WithAcquireActionEventHandler property instead.")]
            public TransactionConfigurationBuilder WithStampSheetEventHandler(Action<CacheDatabase, string, int?, string, string, string> stampSheetEventHandler) {
                this.AcquireActionEventHandler = stampSheetEventHandler;
                return this;
            }
            public TransactionConfigurationBuilder WithVerifyActionEventHandler(Action<CacheDatabase, string, int?, string, string, string> verifyActionEventHandler) {
                this.VerifyActionEventHandler = verifyActionEventHandler;
                return this;
            }
            public TransactionConfigurationBuilder WithConsumeActionEventHandler(Action<CacheDatabase, string, int?, string, string, string> consumeActionEventHandler) {
                this.ConsumeActionEventHandler = consumeActionEventHandler;
                return this;
            }
            public TransactionConfigurationBuilder WithAcquireActionEventHandler(Action<CacheDatabase, string, int?, string, string, string> acquireActionEventHandler) {
                this.AcquireActionEventHandler = acquireActionEventHandler;
                return this;
            }

            public TransactionConfiguration Build() {
                return new TransactionConfiguration(
                    NamespaceName,
                    VerifyActionEventHandler,
                    ConsumeActionEventHandler,
                    AcquireActionEventHandler
                );
            }
        }
    }
}