/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0(the "License").
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

using Gs2.Util.LitJson;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

#if UNITY_2017_1_OR_NEWER
using UnityEngine.Scripting;
#endif

namespace Gs2.Core.Model
{
    internal sealed class NotificationPayloadException : System.Exception
    {
        internal NotificationPayloadException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }

    internal static class NotificationPayload
    {
        private static readonly Regex NumberLiteral = new Regex(
            @"\A(-?)(0|[1-9][0-9]*)(?:\.([0-9]+))?(?:[eE]([+-]?[0-9]+))?\z",
            RegexOptions.CultureInvariant
        );

        private static string NormalizeInteger(string literal)
        {
            var match = NumberLiteral.Match(literal);
            if (!match.Success)
            {
                throw new InvalidOperationException("Notification field was not a JSON number.");
            }
            var fraction = match.Groups[3].Value;
            var digits = (match.Groups[2].Value + fraction).TrimStart('0');
            if (digits.Length == 0) return "0";
            var exponent = match.Groups[4].Success
                ? BigInteger.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture)
                : BigInteger.Zero;
            var scale = exponent - fraction.Length;
            if (scale < 0)
            {
                var removed = -scale;
                if (removed >= digits.Length)
                {
                    throw new InvalidOperationException("Notification field was not an integer.");
                }
                var length = digits.Length - (int)removed;
                for (var i = length; i < digits.Length; i++)
                {
                    if (digits[i] != '0')
                    {
                        throw new InvalidOperationException("Notification field was not an integer.");
                    }
                }
                digits = digits.Substring(0, length);
            }
            else
            {
                // Bound the result before allocating; even an enormous exponent
                // only needs a range check, never a huge power of ten.
                if (digits.Length + scale > 19)
                {
                    throw new OverflowException("Notification integer exceeded the range of a long.");
                }
                digits += new string('0', (int)scale);
            }
            var integer = match.Groups[1].Value + digits;
            if (!long.TryParse(integer, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var value))
            {
                throw new OverflowException("Notification integer exceeded the range of a long.");
            }
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private static int SkipWhitespace(string payload, int offset)
        {
            while (offset < payload.Length && char.IsWhiteSpace(payload[offset])) offset++;
            return offset;
        }

        private static string NormalizeIntegerFields(string payload, string[] integerFields)
        {
            if (integerFields == null || integerFields.Length == 0) return payload;
            var fields = new HashSet<string>(integerFields, StringComparer.Ordinal);
            var result = new StringBuilder();
            var copied = 0;
            var depth = 0;
            for (var offset = 0; offset < payload.Length;)
            {
                if (payload[offset] == '"')
                {
                    var keyStart = offset++;
                    while (offset < payload.Length)
                    {
                        if (payload[offset] == '\\') offset += 2;
                        else if (payload[offset++] == '"') break;
                    }
                    var keyEnd = offset;
                    var colon = SkipWhitespace(payload, offset);
                    if (depth != 1 || colon >= payload.Length || payload[colon] != ':') continue;
                    var keyLiteral = payload.Substring(keyStart, keyEnd - keyStart);
                    // Decode escaped property names with the same JSON reader.
                    // Only the key is read here, before any numeric information is lost.
                    var key = (string)JsonMapper.ToObject("{\"key\":" + keyLiteral + "}")["key"];
                    if (!fields.Contains(key)) continue;
                    var start = SkipWhitespace(payload, colon + 1);
                    if (start >= payload.Length ||
                        !(payload[start] == '-' || (payload[start] >= '0' && payload[start] <= '9'))) continue;
                    offset = start + 1;
                    while (offset < payload.Length &&
                           ((payload[offset] >= '0' && payload[offset] <= '9') ||
                            payload[offset] == '.' || payload[offset] == 'e' || payload[offset] == 'E' ||
                            payload[offset] == '+' || payload[offset] == '-')) offset++;
                    var integer = NormalizeInteger(payload.Substring(start, offset - start));
                    result.Append(payload, copied, start - copied);
                    result.Append(integer);
                    copied = offset;
                    continue;
                }
                if (payload[offset] == '{' || payload[offset] == '[') depth++;
                else if (payload[offset] == '}' || payload[offset] == ']') depth--;
                offset++;
            }
            result.Append(payload, copied, payload.Length - copied);
            return result.ToString();
        }

        // Notifications contain resolved server values. Unlike stamp-sheet replay,
        // a present but unreadable scalar is a protocol error, not an absent value.
        internal static double ReadDouble(JsonData value)
        {
            double number;
            if (value.IsInt) number = (int)value;
            else if (value.IsLong) number = (long)value;
            else if (value.IsDouble) number = (double)value;
            else throw new InvalidOperationException("Notification field was not a JSON number.");

            if (double.IsNaN(number) || double.IsInfinity(number))
            {
                throw new InvalidOperationException("Notification field was not a finite number.");
            }
            return number;
        }

        internal static int ReadInt(JsonData value)
        {
            if (value.IsInt) return (int)value;
            if (value.IsLong) return checked((int)(long)value);
            var number = ReadDouble(value);
            if (number != Math.Truncate(number))
            {
                throw new InvalidOperationException("Notification field was not an integer.");
            }
            return checked((int)number);
        }

        internal static long ReadLong(JsonData value)
        {
            if (value.IsInt) return (int)value;
            if (value.IsLong) return (long)value;
            var number = ReadDouble(value);
            if (number != Math.Truncate(number))
            {
                throw new InvalidOperationException("Notification field was not an integer.");
            }
            // Binary64 rounds values just outside Int64's range to its endpoints.
            // Exact endpoint literals use IsLong and have already returned above.
            if (number <= long.MinValue || number >= (double)long.MaxValue)
            {
                throw new OverflowException("Notification field exceeded the range of a long.");
            }
            return checked((long)number);
        }

        internal static float ReadFloat(JsonData value)
        {
            var number = (float)ReadDouble(value);
            if (float.IsInfinity(number))
            {
                throw new OverflowException("Notification field exceeded the range of a float.");
            }
            return number;
        }

        internal static T Parse<T>(string payload, Func<JsonData, T> parser, string[] integerFields)
            where T : class
        {
            try
            {
                var data = JsonMapper.ToObject(NormalizeIntegerFields(payload, integerFields));
                if (data == null || !data.IsObject)
                {
                    throw new InvalidOperationException("Notification payload was not a JSON object.");
                }
                return parser(data) ??
                       throw new InvalidOperationException("Notification payload could not be parsed.");
            }
            catch (NotificationPayloadException)
            {
                throw;
            }
            catch (System.Exception exception)
            {
                throw new NotificationPayloadException(
                    "Failed to parse a built-in notification payload.",
                    exception
                );
            }
        }
    }

#if UNITY_2017_1_OR_NEWER
    [Preserve]
#endif
    public class NotificationMessage
    {
        public string issuer;
        
        public string subject;
                
        public string payload;

        public static NotificationMessage FromJson(JsonData data)
        {
            return new NotificationMessage
            {
                issuer = data.Keys.Contains("issuer") ? (string)data["issuer"] : null,
                subject = data.Keys.Contains("subject") ? (string)data["subject"] : null,
                payload = data.Keys.Contains("payload") ? (string)data["payload"] : null,
            };
        }
    }
}
