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
using Gs2.Gs2Stamina.Model;
using Gs2.Util.LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Stamina.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateRecoverValueTableMasterRequest : Gs2Request<CreateRecoverValueTableMasterRequest>
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
        public CreateRecoverValueTableMasterRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** スタミナ回復量テーブル名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * スタミナ回復量テーブル名を設定
         *
         * @param name スタミナ回復量テーブル名
         * @return this
         */
        public CreateRecoverValueTableMasterRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** スタミナ回復量テーブルマスターの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * スタミナ回復量テーブルマスターの説明を設定
         *
         * @param description スタミナ回復量テーブルマスターの説明
         * @return this
         */
        public CreateRecoverValueTableMasterRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** スタミナ回復量テーブルのメタデータ */
		[UnityEngine.SerializeField]
        public string metadata;

        /**
         * スタミナ回復量テーブルのメタデータを設定
         *
         * @param metadata スタミナ回復量テーブルのメタデータ
         * @return this
         */
        public CreateRecoverValueTableMasterRequest WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }


        /** 経験値の種類マスター のGRN */
		[UnityEngine.SerializeField]
        public string experienceModelId;

        /**
         * 経験値の種類マスター のGRNを設定
         *
         * @param experienceModelId 経験値の種類マスター のGRN
         * @return this
         */
        public CreateRecoverValueTableMasterRequest WithExperienceModelId(string experienceModelId) {
            this.experienceModelId = experienceModelId;
            return this;
        }


        /** ランク毎のスタミナ回復量テーブル */
		[UnityEngine.SerializeField]
        public List<int?> values;

        /**
         * ランク毎のスタミナ回復量テーブルを設定
         *
         * @param values ランク毎のスタミナ回復量テーブル
         * @return this
         */
        public CreateRecoverValueTableMasterRequest WithValues(List<int?> values) {
            this.values = values;
            return this;
        }


    	[Preserve]
        public static CreateRecoverValueTableMasterRequest FromDict(JsonData data)
        {
            return new CreateRecoverValueTableMasterRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                metadata = data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString(): null,
                experienceModelId = data.Keys.Contains("experienceModelId") && data["experienceModelId"] != null ? data["experienceModelId"].ToString(): null,
                values = data.Keys.Contains("values") && data["values"] != null ? data["values"].Cast<JsonData>().Select(value =>
                    {
                        return (int?)int.Parse(value.ToString());
                    }
                ).ToList() : null,
            };
        }

        public JsonData ToDict()
        {
            var data = new JsonData();
            data["namespaceName"] = namespaceName;
            data["name"] = name;
            data["description"] = description;
            data["metadata"] = metadata;
            data["experienceModelId"] = experienceModelId;
            data["values"] = new JsonData(values);
            return data;
        }
	}
}