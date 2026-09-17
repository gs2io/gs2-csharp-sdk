/*
 * Copyright 2016-2025 Game Server Services, Inc. or its affiliates. All Rights Reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
 * documentation files (the "Software"), to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
 * and to permit projects to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions
 * of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

using System.Globalization;

namespace Gs2.Core.Util
{
    /// <summary>
    /// Lenient readers for JSON scalars.
    ///
    /// `FromJson` is not only used on API responses: the SDK also replays the
    /// request recorded in a stamp sheet to update its local cache. Those
    /// requests may still carry transaction placeholders such as
    /// <c>#{slot}</c>, which the server substitutes but the client never sees
    /// resolved. Parsing such a value strictly would throw and abandon the
    /// whole cache update, so an unreadable scalar is reported as absent
    /// instead — the field's value is genuinely unknown to the client.
    /// </summary>
    public static class JsonValue
    {
        public static int? ToNullableInt(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var asDouble))
            {
                return (int)asDouble;
            }
            return null;
        }

        public static long? ToNullableLong(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed;
            }
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var asDouble))
            {
                return (long)asDouble;
            }
            return null;
        }

        public static float? ToNullableFloat(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : (float?)null;
        }

        public static double? ToNullableDouble(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : (double?)null;
        }

        public static bool? ToNullableBool(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return bool.TryParse(value, out var parsed) ? parsed : (bool?)null;
        }
    }
}
