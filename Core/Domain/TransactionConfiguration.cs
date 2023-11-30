using System;

namespace Gs2.Core.Domain
{
    
    
    public class TransactionConfiguration
    {
        public string NamespaceName { get; set; }
        public Action<CacheDatabase, string, string, string, string> StampTaskEventHandler { get; set; }
        public Action<CacheDatabase, string, string, string, string> StampSheetEventHandler { get; set; }

        private TransactionConfiguration(
            string namespaceName,
            Action<CacheDatabase, string, string, string, string> stampTaskEventHandler,
            Action<CacheDatabase, string, string, string, string> stampSheetEventHandler
        ) {
            this.NamespaceName = namespaceName;
            this.StampTaskEventHandler = stampTaskEventHandler;
            this.StampSheetEventHandler = stampSheetEventHandler;
        }

        public static TransactionConfigurationBuilder Builder() {
            return new TransactionConfigurationBuilder();
        }

        public class TransactionConfigurationBuilder {

            private string NamespaceName { get; set; }
            private Action<CacheDatabase, string, string, string, string> StampTaskEventHandler { get; set; }
            private Action<CacheDatabase, string, string, string, string> StampSheetEventHandler { get; set; }

            internal TransactionConfigurationBuilder(){}

            public TransactionConfigurationBuilder WithNamespaceName(string namespaceName) {
                this.NamespaceName = namespaceName;
                return this;
            }
            public TransactionConfigurationBuilder WithStampTaskEventHandler(Action<CacheDatabase, string, string, string, string> stampTaskEventHandler) {
                this.StampTaskEventHandler = stampTaskEventHandler;
                return this;
            }
            public TransactionConfigurationBuilder WithStampSheetEventHandler(Action<CacheDatabase, string, string, string, string> stampSheetEventHandler) {
                this.StampSheetEventHandler = stampSheetEventHandler;
                return this;
            }

            public TransactionConfiguration Build() {
                return new TransactionConfiguration(
                    NamespaceName,
                    StampTaskEventHandler,
                    StampSheetEventHandler
                );
            }
        }
    }
}