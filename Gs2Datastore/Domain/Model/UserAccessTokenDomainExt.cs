using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_2017_1_OR_NEWER
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using UnityEngine.Events;
using UnityEngine.Networking;
#else
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
#endif
using Gs2.Core;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Datastore.Model;
using Gs2.Gs2Datastore.Request;
using Gs2.Gs2Datastore.Result;

// ReSharper disable once CheckNamespace
namespace Gs2.Gs2Datastore.Domain.Model
{
    public partial class UserAccessTokenDomain
    {
        
#if UNITY_2017_1_OR_NEWER
	#if GS2_ENABLE_UNITASK
	    public async UniTask<DataObject> Upload(
	#else
	    private class UploadImplFuture : Gs2Future<RestResult>
	    {
		    private UserAccessTokenDomain _domain;
		    private string _uploadUrl;
		    private byte[] _data;

		    public UploadImplFuture(
			    UserAccessTokenDomain domain,
			    string uploadUrl,
			    byte[] data
		    )
		    {
			    _domain = domain;
			    _uploadUrl = uploadUrl;
			    _data = data;
		    }

		    public override IEnumerator Action()
		    {
			    using var request = UnityWebRequest.Put(_uploadUrl, _data);
			    request.downloadHandler = new DownloadHandlerBuffer();
			    yield return request.SendWebRequest();
			    OnComplete(new RestResult(
				    (int) request.responseCode,
				    request.responseCode == 200 ? "{}" : string.IsNullOrEmpty(request.error) ? "{}" : request.error
			    ));
		    }
	    }

	    public class UploadFuture : Gs2Future<DataObject>
	    {
		    private readonly UserAccessTokenDomain _domain;
		    private readonly string _scope;
		    private readonly List<string> _allowUserIds;
		    private readonly byte[] _data;
		    private readonly string _name;
		    private readonly bool? _updateIfExists;

		    public UploadFuture(
			    UserAccessTokenDomain domain,
			    string scope,
			    List<string> allowUserIds,
			    byte[] data,
			    string name=null,
			    bool? updateIfExists=null
		    )
		    {
			    this._domain = domain;
			    this._scope = scope;
			    this._allowUserIds = allowUserIds;
			    this._data = data;
			    this._name = name;
			    this._updateIfExists = updateIfExists;
		    }

		    public override IEnumerator Action()
		    {
			    {
				    string dataObjectName;
				    string uploadUrl;
				    {
					    var task = this._domain.PrepareUploadFuture(
						    new PrepareUploadRequest()
							    .WithScope(this._scope)
							    .WithAllowUserIds(this._allowUserIds.ToArray())
							    .WithName(this._name)
							    .WithUpdateIfExists(this._updateIfExists)
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
						var task2 = task.Result.ModelFuture();
						yield return task2;

					    dataObjectName = task2.Result.Name;
					    uploadUrl = task.Result.UploadUrl;
				    }
				    {
					    var task = new UploadImplFuture(
						    this._domain,
						    uploadUrl,
						    this._data
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
				    }
				    {
					    var task = this._domain.DataObject(
						    dataObjectName
					    ).DoneUploadFuture(
						    new DoneUploadRequest()
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
						var task2 = task.Result.ModelFuture();
						yield return task2;
					    OnComplete(task2.Result);
				    }
			    }
		    }
	    }

        public UploadFuture Upload(
	#endif
#else
	    public async Task<DataObjectAccessTokenDomain> Upload(
#endif
	        string scope,
	        List<string> allowUserIds,
	        byte[] data,
	        string name=null,
	        bool? updateIfExists=null
        )
        {
#if UNITY_2017_1_OR_NEWER
	#if GS2_ENABLE_UNITASK
	        string dataObjectName = null;
	        {
		        var result = await this.PrepareUploadAsync(
			        new PrepareUploadRequest()
				        .WithScope(scope)
				        .WithAllowUserIds(allowUserIds.ToArray())
				        .WithName(name)
				        .WithUpdateIfExists(updateIfExists)
		        );
		        dataObjectName = result.DataObjectName;
		        
		        using var request = UnityWebRequest.Put(result.UploadUrl, data);
		        request.downloadHandler = new DownloadHandlerBuffer();
		        await request.SendWebRequest();
		        if (request.responseCode != 200)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
	        }
	        {
		        var result = await (await this.DataObject(
			        dataObjectName
			    ).DoneUploadAsync(
			        new DoneUploadRequest()
		        )).ModelAsync();
		        return result;
	        }
	#else
	        return new UploadFuture(
		        this,
		        scope,
		        allowUserIds,
		        data,
		        name,
		        updateIfExists
	        );
	#endif
#else
	        string dataObjectName = null;
	        {
		        var result = await this.PrepareUploadAsync(
			        new PrepareUploadRequest()
				        .WithScope(scope)
				        .WithAllowUserIds(allowUserIds.ToArray())
				        .WithName(name)
				        .WithUpdateIfExists(updateIfExists)
		        );
		        dataObjectName = result.DataObjectName;
		        
		        var response = await new HttpClient().PutAsync(result.UploadUrl, new ByteArrayContent(data));
		        if (response.StatusCode != HttpStatusCode.OK)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
	        }
	        {
		        var result = await this.DataObject(
			        dataObjectName
		        ).DoneUploadAsync(
			        new DoneUploadRequest()
		        );
		        return result;
	        }
#endif
        }
    }
}