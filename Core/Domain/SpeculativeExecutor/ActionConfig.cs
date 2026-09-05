using Gs2.Util.LitJson;

namespace Gs2.Core.SpeculativeExecutor
{
    internal static class ActionConfig
    {
        internal static string EscapeJsonStringContent(string value)
        {
            var serialized = new JsonData(value).ToJson();
            return serialized.Substring(1, serialized.Length - 2);
        }
    }
}
