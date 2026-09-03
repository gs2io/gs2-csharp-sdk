using System.Numerics;
using System.Linq;
using Gs2.Core.Model;

namespace Gs2.Core.SpeculativeExecutor
{
    public static class Transaction
    {
        /// <summary>
        /// Returns null because an inverse acquire action cannot be inferred
        /// from an opaque action name and JSON request. Use acquire actions
        /// precomputed by the service instead.
        /// </summary>
        public static AcquireAction[] Revert(ConsumeAction[] actions) {
            return null;
        }

        /// <summary>
        /// Returns null because this legacy signature cannot represent the
        /// consume actions required to reverse acquire actions. Use inverse
        /// actions precomputed by the service instead.
        /// </summary>
        public static AcquireAction[] Revert(AcquireAction[] actions) {
            return null;
        }
        
        public static AcquireAction[] Rate(AcquireAction[] actions, double rate) {
            return Clone(actions);
        }
        
        public static ConsumeAction[] Rate(ConsumeAction[] actions, double rate) {
            return Clone(actions);
        }
        
        public static AcquireAction[] Rate(AcquireAction[] actions, BigInteger rate) {
            return Clone(actions);
        }
        
        public static ConsumeAction[] Rate(ConsumeAction[] actions, BigInteger rate) {
            return Clone(actions);
        }

        private static AcquireAction[] Clone(AcquireAction[] actions) {
            return actions?.Select(action => action?.Clone() as AcquireAction).ToArray();
        }

        private static ConsumeAction[] Clone(ConsumeAction[] actions) {
            return actions?.Select(action => action?.Clone() as ConsumeAction).ToArray();
        }
    }
}
