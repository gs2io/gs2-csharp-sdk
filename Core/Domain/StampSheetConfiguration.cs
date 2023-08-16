using System;

namespace Gs2.Core.Domain
{
    
    
    public class StampSheetConfiguration
    {
        public string NamespaceName { get; set; }
        public Action<CacheDatabase, string, string, string, string> StampTaskEventHandler { get; set; }
        public Action<CacheDatabase, string, string, string, string> StampSheetEventHandler { get; set; }

        private StampSheetConfiguration(
            string namespaceName,
            Action<CacheDatabase, string, string, string, string> stampTaskEventHandler,
            Action<CacheDatabase, string, string, string, string> stampSheetEventHandler
        ) {
            this.NamespaceName = namespaceName;
            this.StampTaskEventHandler = stampTaskEventHandler;
            this.StampSheetEventHandler = stampSheetEventHandler;
        }

        public static StampSheetConfigurationBuilder Builder() {
            return new StampSheetConfigurationBuilder();
        }

        public class StampSheetConfigurationBuilder {

            public string NamespaceName { get; set; }

            internal StampSheetConfigurationBuilder(){}

            public StampSheetConfigurationBuilder WithNamespaceName(string namespaceName) {
                this.NamespaceName = namespaceName;
                return this;
            }

            public StampSheetConfiguration Build() {
                return new StampSheetConfiguration(
                    NamespaceName,
                    Gs2.UpdateCacheFromStampTask,
                    Gs2.UpdateCacheFromStampSheet
                );
            }
        }
    }
}