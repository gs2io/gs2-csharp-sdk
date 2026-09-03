/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 * deny overwrite
 */

// ReSharper disable ConvertSwitchStatementToSwitchExpression

#pragma warning disable CS1522 // Empty switch block

using System;
using System.Collections.Generic; /* diff +++ */
using System.Linq;
using System.Numerics;
/* diff +++ start */
using System.Text;
using System.Text.RegularExpressions;
/* diff +++ end */
using Gs2.Core.Exception;
using Gs2.Gs2Grade.Request;

namespace Gs2.Gs2Grade.Model.Transaction
{
    public static partial class StatusExt
    {
/* diff +++ start */
        private static readonly TimeSpan GradeUpMaterialRegexTimeout =
            TimeSpan.FromMilliseconds(100);

        private static bool IsAscii(string value) {
            if (value == null) {
                return false;
            }
            for (var i = 0; i < value.Length; i++) {
                if (value[i] > 0x7f) {
                    return false;
                }
            }
            return true;
        }

        private static bool TryPreparePortableRegex(
            string pattern,
            out string prepared
        ) {
            prepared = null;
            if (pattern == null) {
                return false;
            }
            var result = new StringBuilder();
            var namedGroups = new HashSet<string>();
            var hasNamedGroup = false;
            var hasUnnamedCapture = false;
            var inCharacterClass = false;
            var characterClassHasContent = false;
            var characterClassPrevious = '\0';
            var characterClassPreviousCanStartRange = false;
            var characterClassRangeEndpoint = -1;
            var finiteQuantifierCount = 0;
            for (var i = 0; i < pattern.Length; i++) {
                var current = pattern[i];
                if (current > 0x7f) {
                    return false;
                }
                if (current == '\\') {
                    if (inCharacterClass) {
                        return false;
                    }
                    if (i + 1 >= pattern.Length) {
                        result.Append(current);
                        continue;
                    }
                    var escaped = pattern[++i];
                    if (escaped > 0x7f ||
                        !("\\.^$|?*+()[]{}-afnrtvAz".IndexOf(escaped) >= 0)) {
                        return false;
                    }
                    result.Append('\\').Append(escaped);
                    continue;
                }
                if (current == '[') {
                    if (inCharacterClass ||
                        (i + 1 < pattern.Length && pattern[i + 1] == '[')) {
                        return false;
                    }
                    inCharacterClass = true;
                    characterClassHasContent = false;
                    characterClassPrevious = '\0';
                    characterClassPreviousCanStartRange = false;
                    characterClassRangeEndpoint = -1;
                    result.Append(current);
                    continue;
                }
                if (current == ']' && inCharacterClass) {
                    if (!characterClassHasContent) {
                        return false;
                    }
                    inCharacterClass = false;
                    result.Append(current);
                    continue;
                }
                if (inCharacterClass) {
                    if (current == '^' && !characterClassHasContent) {
                        result.Append(current);
                        continue;
                    }
                    if (current == '^') {
                        return false;
                    }
                    if ((current == '&' && i + 1 < pattern.Length &&
                         pattern[i + 1] == '&') ||
                        (current == ':' && i > 0 && pattern[i - 1] == '[')) {
                        return false;
                    }
                    if (current == '-') {
                        if (!characterClassPreviousCanStartRange ||
                            i + 1 >= pattern.Length ||
                            !((characterClassPrevious >= '0' &&
                               characterClassPrevious <= '9') ||
                              (characterClassPrevious >= 'a' &&
                               characterClassPrevious <= 'z') ||
                              (characterClassPrevious >= 'A' &&
                               characterClassPrevious <= 'Z')) ||
                            !((pattern[i + 1] >= '0' &&
                               pattern[i + 1] <= '9') ||
                              (pattern[i + 1] >= 'a' &&
                               pattern[i + 1] <= 'z') ||
                              (pattern[i + 1] >= 'A' &&
                               pattern[i + 1] <= 'Z')) ||
                            pattern[i + 1] < characterClassPrevious) {
                            return false;
                        }
                        characterClassRangeEndpoint = i + 1;
                        characterClassPreviousCanStartRange = false;
                        result.Append(current);
                        continue;
                    }
                    characterClassHasContent = true;
                    characterClassPrevious = current;
                    characterClassPreviousCanStartRange =
                        i != characterClassRangeEndpoint;
                    if (i == characterClassRangeEndpoint) {
                        characterClassRangeEndpoint = -1;
                    }
                    result.Append(current);
                    continue;
                }
                if (current == '(' && i + 1 < pattern.Length &&
                    pattern[i + 1] == '?') {
                    if (i + 2 < pattern.Length && pattern[i + 2] == ':') {
                        result.Append("(?:");
                        i += 2;
                        continue;
                    }
                    if (i + 3 < pattern.Length && pattern[i + 2] == 'P' &&
                        pattern[i + 3] == '<') {
                        var nameEnd = i + 4;
                        while (nameEnd < pattern.Length &&
                               ((pattern[nameEnd] >= 'a' &&
                                 pattern[nameEnd] <= 'z') ||
                                (pattern[nameEnd] >= 'A' &&
                                 pattern[nameEnd] <= 'Z') ||
                                (pattern[nameEnd] >= '0' &&
                                 pattern[nameEnd] <= '9') ||
                                pattern[nameEnd] == '_')) {
                            nameEnd++;
                        }
                        if (nameEnd == i + 4 || nameEnd >= pattern.Length ||
                            pattern[nameEnd] != '>') {
                            return false;
                        }
                        var groupName = pattern.Substring(
                            i + 4,
                            nameEnd - i - 4
                        );
                        if (!((groupName[0] >= 'a' &&
                               groupName[0] <= 'z') ||
                              (groupName[0] >= 'A' &&
                               groupName[0] <= 'Z')) ||
                            !namedGroups.Add(groupName)) {
                            return false;
                        }
                        hasNamedGroup = true;
                        result.Append("(?<")
                            .Append(groupName)
                            .Append('>');
                        i = nameEnd;
                        continue;
                    }
                    return false;
                }
                if (current == '(') {
                    hasUnnamedCapture = true;
                }
                if (current == '$') {
                    result.Append("\\z");
                    continue;
                }
                if (current == '{') {
                    finiteQuantifierCount++;
                    if (finiteQuantifierCount > 1) {
                        return false;
                    }
                    var quantifierEnd = i + 1;
                    var digitsBeforeComma = 0;
                    var lowerBound = 0;
                    while (quantifierEnd < pattern.Length &&
                           pattern[quantifierEnd] >= '0' &&
                           pattern[quantifierEnd] <= '9') {
                        digitsBeforeComma++;
                        lowerBound = lowerBound > 1000
                            ? 1001
                            : lowerBound * 10 +
                              pattern[quantifierEnd] - '0';
                        quantifierEnd++;
                    }
                    if (digitsBeforeComma == 0 || lowerBound > 1000 ||
                        (digitsBeforeComma > 1 && pattern[i + 1] == '0')) {
                        return false;
                    }
                    if (quantifierEnd < pattern.Length &&
                        pattern[quantifierEnd] == ',') {
                        quantifierEnd++;
                        var upperBoundStart = quantifierEnd;
                        var upperBound = 0;
                        while (quantifierEnd < pattern.Length &&
                               pattern[quantifierEnd] >= '0' &&
                               pattern[quantifierEnd] <= '9') {
                            upperBound = upperBound > 1000
                                ? 1001
                                : upperBound * 10 +
                                  pattern[quantifierEnd] - '0';
                            quantifierEnd++;
                        }
                        if (upperBound > 1000 ||
                            (quantifierEnd - upperBoundStart > 1 &&
                             pattern[upperBoundStart] == '0')) {
                            return false;
                        }
                    }
                    if (quantifierEnd >= pattern.Length ||
                        pattern[quantifierEnd] != '}') {
                        return false;
                    }
                    result.Append(
                        pattern,
                        i,
                        quantifierEnd - i + 1
                    );
                    i = quantifierEnd;
                    continue;
                }
                result.Append(current);
            }
            if (inCharacterClass) {
                prepared = result.ToString();
                return true;
            }
            if (hasNamedGroup && hasUnnamedCapture) {
                return false;
            }
            prepared = result.ToString();
            return true;
        }

        internal static bool TryMatchPortableRegex(
            string value,
            string pattern,
            out bool matches
        ) {
            matches = false;
            if (!IsAscii(value) ||
                !TryPreparePortableRegex(pattern, out var prepared)) {
                return false;
            }
            try {
                matches = Regex.IsMatch(
                    value,
                    prepared,
                    RegexOptions.CultureInvariant,
                    GradeUpMaterialRegexTimeout
                );
                return true;
            }
            catch (ArgumentException) {
                matches = false;
                return true;
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
        }

        private static string ExpandGoReplacement(
            Match match,
            Regex regex,
            string template
        ) {
            var result = new StringBuilder();
            for (var i = 0; i < template.Length;) {
                if (template[i] != '$') {
                    result.Append(template[i++]);
                    continue;
                }
                if (i + 1 < template.Length && template[i + 1] == '$') {
                    result.Append('$');
                    i += 2;
                    continue;
                }

                var nameStart = i + 1;
                var braced = nameStart < template.Length &&
                             template[nameStart] == '{';
                if (braced) {
                    nameStart++;
                }
                var nameEnd = nameStart;
                while (nameEnd < template.Length &&
                       (char.IsLetterOrDigit(template[nameEnd]) ||
                        template[nameEnd] == '_')) {
                    nameEnd++;
                }
                if (nameEnd == nameStart ||
                    (braced &&
                     (nameEnd >= template.Length ||
                      template[nameEnd] != '}'))) {
                    result.Append('$');
                    i++;
                    continue;
                }

                var name = template.Substring(
                    nameStart,
                    nameEnd - nameStart
                );
                i = nameEnd + (braced ? 1 : 0);

                var groupNumber = -1;
                var numeric = true;
                var parsedNumber = 0;
                for (var j = 0; j < name.Length; j++) {
                    if (name[j] < '0' || name[j] > '9' ||
                        parsedNumber >= 100000000) {
                        numeric = false;
                        break;
                    }
                    parsedNumber = parsedNumber * 10 + name[j] - '0';
                }
                if (name.Length > 1 && name[0] == '0') {
                    numeric = false;
                }
                if (numeric) {
                    groupNumber = parsedNumber;
                }
                else {
                    groupNumber = regex.GroupNumberFromName(name);
                }

                if (groupNumber >= 0 &&
                    groupNumber < match.Groups.Count &&
                    match.Groups[groupNumber].Success) {
                    result.Append(match.Groups[groupNumber].Value);
                }
            }
            return result.ToString();
        }

        private static string ReplaceAllGo(
            Regex regex,
            string source,
            string replacement
        ) {
            var result = new StringBuilder();
            var lastMatchEnd = 0;
            foreach (Match match in regex.Matches(source)) {
                result.Append(
                    source,
                    lastMatchEnd,
                    match.Index - lastMatchEnd
                );
                if (match.Index + match.Length > lastMatchEnd ||
                    match.Index == 0) {
                    result.Append(ExpandGoReplacement(
                        match,
                        regex,
                        replacement
                    ));
                }
                lastMatchEnd = match.Index + match.Length;
            }
            result.Append(
                source,
                lastMatchEnd,
                source.Length - lastMatchEnd
            );
            return result.ToString();
        }

/* diff +++ end */
        public static bool IsExecutable(
            this Status self,
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
/* diff +++ start */
            return self.IsExecutable(request, null);
        }

        public static bool IsExecutable(
            this Status self,
            VerifyGradeUpMaterialByUserIdRequest request,
            GradeModel gradeModel
        ) {
            return self.TryEvaluateGradeUpMaterial(
                request,
                gradeModel,
                out var executable
            ) && executable;
        }

        internal static bool TryEvaluateGradeUpMaterial(
            this Status self,
            VerifyGradeUpMaterialByUserIdRequest request,
            GradeModel gradeModel,
            out bool executable
        ) {
            executable = false;
            if (self?.GradeValue == null ||
                string.IsNullOrEmpty(request?.PropertyId) ||
                string.IsNullOrEmpty(request.MaterialPropertyId) ||
                gradeModel?.GradeEntries == null ||
                gradeModel.GradeEntries.Length == 0 ||
                self.GradeValue.Value <= 0) {
                return false;
            }

            var gradeEntryIndex = (int)Math.Min(
                gradeModel.GradeEntries.LongLength - 1,
                self.GradeValue.Value - 1
            );
            var gradeEntry = gradeModel.GradeEntries[gradeEntryIndex];
            if (string.IsNullOrEmpty(gradeEntry?.PropertyIdRegex) ||
                string.IsNullOrEmpty(gradeEntry.GradeUpPropertyIdRegex)) {
                return false;
            }
            if (!IsAscii(request.PropertyId) ||
                !IsAscii(request.MaterialPropertyId) ||
                !IsAscii(gradeEntry.PropertyIdRegex) ||
                !IsAscii(gradeEntry.GradeUpPropertyIdRegex)) {
                return false;
            }

            var materialPropertyIdRegex = request.PropertyId;
            if (!TryPreparePortableRegex(
                    gradeEntry.PropertyIdRegex,
                    out var preparedPropertyRegex)) {
                return false;
            }
            try {
                var propertyRegex = new Regex(
                    preparedPropertyRegex,
                    RegexOptions.CultureInvariant,
                    GradeUpMaterialRegexTimeout
                );
                materialPropertyIdRegex = ReplaceAllGo(
                    propertyRegex,
                    request.PropertyId,
                    gradeEntry.GradeUpPropertyIdRegex
                );
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
            catch (ArgumentException) {
                // The service keeps the source property ID when the
                // pattern cannot be compiled.
            }

            bool matches;
            if (!TryPreparePortableRegex(
                    materialPropertyIdRegex,
                    out var preparedMaterialRegex)) {
                return false;
            }
            try {
                matches = Regex.IsMatch(
                    request.MaterialPropertyId,
                    preparedMaterialRegex,
                    RegexOptions.CultureInvariant,
                    GradeUpMaterialRegexTimeout
                );
            }
            catch (RegexMatchTimeoutException) {
                return false;
            }
            catch (ArgumentException) {
                matches = false;
            }

/* diff +++ end */
            switch (request.VerifyType) {
                case "match":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Grade:VerifyGradeUpMaterialByUserId");
 diff --- end */
/* diff +++ start */
                    executable = matches;
                    return true;
/* diff +++ end */
                case "notMatch":
/* diff --- start
                    throw new NotImplementedException($"not implemented action Gs2Grade:VerifyGradeUpMaterialByUserId");
 diff --- end */
/* diff +++ start */
                    executable = !matches;
                    return true;
/* diff +++ end */
            }
/* diff --- start
            return false;
 diff --- end */
            return true; /* diff +++ */
        }

        public static Status SpeculativeExecution(
            this Status self,
            VerifyGradeUpMaterialByUserIdRequest request
        ) {
            return self.Clone() as Status;
        }

        public static VerifyGradeUpMaterialByUserIdRequest Rate(
            this VerifyGradeUpMaterialByUserIdRequest request,
            double rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Grade:VerifyGradeUpMaterialByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }

    public static partial class VerifyGradeUpMaterialByUserIdRequestExt
    {
        public static VerifyGradeUpMaterialByUserIdRequest Rate(
            this VerifyGradeUpMaterialByUserIdRequest request,
            BigInteger rate
        ) {
/* diff --- start
            throw new NotSupportedException($"not supported rate action Gs2Grade:VerifyGradeUpMaterialByUserId");
 diff --- end */
            return request; /* diff +++ */
        }
    }
}