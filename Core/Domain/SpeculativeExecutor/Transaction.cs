using System.Numerics;
using Gs2.Core.Model;

namespace Gs2.Core.SpeculativeExecutor
{
    public static class Transaction
    {
        public static AcquireAction[] Revert(ConsumeAction[] actions) {
            return null;
        }
        
        public static AcquireAction[] Revert(AcquireAction[] actions) {
            return null;
        }
        
        public static AcquireAction[] Rate(AcquireAction[] actions, double rate) {
            return actions;
        }
        
        public static ConsumeAction[] Rate(ConsumeAction[] actions, double rate) {
            return actions;
        }
        
        public static AcquireAction[] Rate(AcquireAction[] actions, BigInteger rate) {
            return actions;
        }
        
        public static ConsumeAction[] Rate(ConsumeAction[] actions, BigInteger rate) {
            return actions;
        }
    }
}
