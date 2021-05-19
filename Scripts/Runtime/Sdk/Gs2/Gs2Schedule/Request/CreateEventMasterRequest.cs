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
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Schedule.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Schedule.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateEventMasterRequest : Gs2Request<CreateEventMasterRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペース名を設定
         *
         * @param namespaceName ネームスペース名
         * @return this
         */
        public CreateEventMasterRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** イベントの種類名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * イベントの種類名を設定
         *
         * @param name イベントの種類名
         * @return this
         */
        public CreateEventMasterRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** イベントマスターの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * イベントマスターの説明を設定
         *
         * @param description イベントマスターの説明
         * @return this
         */
        public CreateEventMasterRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** イベントの種類のメタデータ */
		[UnityEngine.SerializeField]
        public string metadata;

        /**
         * イベントの種類のメタデータを設定
         *
         * @param metadata イベントの種類のメタデータ
         * @return this
         */
        public CreateEventMasterRequest WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }


        /** イベント期間の種類 */
		[UnityEngine.SerializeField]
        public string scheduleType;

        /**
         * イベント期間の種類を設定
         *
         * @param scheduleType イベント期間の種類
         * @return this
         */
        public CreateEventMasterRequest WithScheduleType(string scheduleType) {
            this.scheduleType = scheduleType;
            return this;
        }


        /** イベントの開始日時 */
		[UnityEngine.SerializeField]
        public long? absoluteBegin;

        /**
         * イベントの開始日時を設定
         *
         * @param absoluteBegin イベントの開始日時
         * @return this
         */
        public CreateEventMasterRequest WithAbsoluteBegin(long? absoluteBegin) {
            this.absoluteBegin = absoluteBegin;
            return this;
        }


        /** イベントの終了日時 */
		[UnityEngine.SerializeField]
        public long? absoluteEnd;

        /**
         * イベントの終了日時を設定
         *
         * @param absoluteEnd イベントの終了日時
         * @return this
         */
        public CreateEventMasterRequest WithAbsoluteEnd(long? absoluteEnd) {
            this.absoluteEnd = absoluteEnd;
            return this;
        }


        /** 繰り返しの種類 */
		[UnityEngine.SerializeField]
        public string repeatType;

        /**
         * 繰り返しの種類を設定
         *
         * @param repeatType 繰り返しの種類
         * @return this
         */
        public CreateEventMasterRequest WithRepeatType(string repeatType) {
            this.repeatType = repeatType;
            return this;
        }


        /** イベントの繰り返し開始日 */
		[UnityEngine.SerializeField]
        public int? repeatBeginDayOfMonth;

        /**
         * イベントの繰り返し開始日を設定
         *
         * @param repeatBeginDayOfMonth イベントの繰り返し開始日
         * @return this
         */
        public CreateEventMasterRequest WithRepeatBeginDayOfMonth(int? repeatBeginDayOfMonth) {
            this.repeatBeginDayOfMonth = repeatBeginDayOfMonth;
            return this;
        }


        /** イベントの繰り返し終了日 */
		[UnityEngine.SerializeField]
        public int? repeatEndDayOfMonth;

        /**
         * イベントの繰り返し終了日を設定
         *
         * @param repeatEndDayOfMonth イベントの繰り返し終了日
         * @return this
         */
        public CreateEventMasterRequest WithRepeatEndDayOfMonth(int? repeatEndDayOfMonth) {
            this.repeatEndDayOfMonth = repeatEndDayOfMonth;
            return this;
        }


        /** イベントの繰り返し開始曜日 */
		[UnityEngine.SerializeField]
        public string repeatBeginDayOfWeek;

        /**
         * イベントの繰り返し開始曜日を設定
         *
         * @param repeatBeginDayOfWeek イベントの繰り返し開始曜日
         * @return this
         */
        public CreateEventMasterRequest WithRepeatBeginDayOfWeek(string repeatBeginDayOfWeek) {
            this.repeatBeginDayOfWeek = repeatBeginDayOfWeek;
            return this;
        }


        /** イベントの繰り返し終了曜日 */
		[UnityEngine.SerializeField]
        public string repeatEndDayOfWeek;

        /**
         * イベントの繰り返し終了曜日を設定
         *
         * @param repeatEndDayOfWeek イベントの繰り返し終了曜日
         * @return this
         */
        public CreateEventMasterRequest WithRepeatEndDayOfWeek(string repeatEndDayOfWeek) {
            this.repeatEndDayOfWeek = repeatEndDayOfWeek;
            return this;
        }


        /** イベントの繰り返し開始時間 */
		[UnityEngine.SerializeField]
        public int? repeatBeginHour;

        /**
         * イベントの繰り返し開始時間を設定
         *
         * @param repeatBeginHour イベントの繰り返し開始時間
         * @return this
         */
        public CreateEventMasterRequest WithRepeatBeginHour(int? repeatBeginHour) {
            this.repeatBeginHour = repeatBeginHour;
            return this;
        }


        /** イベントの繰り返し終了時間 */
		[UnityEngine.SerializeField]
        public int? repeatEndHour;

        /**
         * イベントの繰り返し終了時間を設定
         *
         * @param repeatEndHour イベントの繰り返し終了時間
         * @return this
         */
        public CreateEventMasterRequest WithRepeatEndHour(int? repeatEndHour) {
            this.repeatEndHour = repeatEndHour;
            return this;
        }


        /** イベントの開始トリガー名 */
		[UnityEngine.SerializeField]
        public string relativeTriggerName;

        /**
         * イベントの開始トリガー名を設定
         *
         * @param relativeTriggerName イベントの開始トリガー名
         * @return this
         */
        public CreateEventMasterRequest WithRelativeTriggerName(string relativeTriggerName) {
            this.relativeTriggerName = relativeTriggerName;
            return this;
        }


        /** イベントの開催期間(秒) */
		[UnityEngine.SerializeField]
        public int? relativeDuration;

        /**
         * イベントの開催期間(秒)を設定
         *
         * @param relativeDuration イベントの開催期間(秒)
         * @return this
         */
        public CreateEventMasterRequest WithRelativeDuration(int? relativeDuration) {
            this.relativeDuration = relativeDuration;
            return this;
        }


    	[Preserve]
        public static CreateEventMasterRequest FromDict(JsonData data)
        {
            return new CreateEventMasterRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                metadata = data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString(): null,
                scheduleType = data.Keys.Contains("scheduleType") && data["scheduleType"] != null ? data["scheduleType"].ToString(): null,
                absoluteBegin = data.Keys.Contains("absoluteBegin") && data["absoluteBegin"] != null ? (long?)long.Parse(data["absoluteBegin"].ToString()) : null,
                absoluteEnd = data.Keys.Contains("absoluteEnd") && data["absoluteEnd"] != null ? (long?)long.Parse(data["absoluteEnd"].ToString()) : null,
                repeatType = data.Keys.Contains("repeatType") && data["repeatType"] != null ? data["repeatType"].ToString(): null,
                repeatBeginDayOfMonth = data.Keys.Contains("repeatBeginDayOfMonth") && data["repeatBeginDayOfMonth"] != null ? (int?)int.Parse(data["repeatBeginDayOfMonth"].ToString()) : null,
                repeatEndDayOfMonth = data.Keys.Contains("repeatEndDayOfMonth") && data["repeatEndDayOfMonth"] != null ? (int?)int.Parse(data["repeatEndDayOfMonth"].ToString()) : null,
                repeatBeginDayOfWeek = data.Keys.Contains("repeatBeginDayOfWeek") && data["repeatBeginDayOfWeek"] != null ? data["repeatBeginDayOfWeek"].ToString(): null,
                repeatEndDayOfWeek = data.Keys.Contains("repeatEndDayOfWeek") && data["repeatEndDayOfWeek"] != null ? data["repeatEndDayOfWeek"].ToString(): null,
                repeatBeginHour = data.Keys.Contains("repeatBeginHour") && data["repeatBeginHour"] != null ? (int?)int.Parse(data["repeatBeginHour"].ToString()) : null,
                repeatEndHour = data.Keys.Contains("repeatEndHour") && data["repeatEndHour"] != null ? (int?)int.Parse(data["repeatEndHour"].ToString()) : null,
                relativeTriggerName = data.Keys.Contains("relativeTriggerName") && data["relativeTriggerName"] != null ? data["relativeTriggerName"].ToString(): null,
                relativeDuration = data.Keys.Contains("relativeDuration") && data["relativeDuration"] != null ? (int?)int.Parse(data["relativeDuration"].ToString()) : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["scheduleType"] = scheduleType;
            data["absoluteBegin"] = absoluteBegin;
            data["absoluteEnd"] = absoluteEnd;
            data["repeatType"] = repeatType;
            data["repeatBeginDayOfMonth"] = repeatBeginDayOfMonth;
            data["repeatEndDayOfMonth"] = repeatEndDayOfMonth;
            data["repeatBeginDayOfWeek"] = repeatBeginDayOfWeek;
            data["repeatEndDayOfWeek"] = repeatEndDayOfWeek;
            data["repeatBeginHour"] = repeatBeginHour;
            data["repeatEndHour"] = repeatEndHour;
            data["relativeTriggerName"] = relativeTriggerName;
            data["relativeDuration"] = relativeDuration;
            return data;
        }
	}
}