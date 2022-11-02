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
			    var request = UnityWebRequest.Put(_uploadUrl, _data);
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
		    private UserAccessTokenDomain _domain;
		    private string _scope;
		    private List<string> _allowUserIds;
		    private byte[] _data;
		    private string _name;
		    private bool? _updateIfExists;

		    public UploadFuture(
			    UserAccessTokenDomain domain,
			    string scope,
			    List<string> allowUserIds,
			    byte[] data,
			    string name=null,
			    bool? updateIfExists=null
		    )
		    {
			    _domain = domain;
			    _scope = scope;
			    _allowUserIds = allowUserIds;
			    _data = data;
			    _name = name;
			    _updateIfExists = updateIfExists;
		    }

		    public override IEnumerator Action()
		    {
			    {
				    string dataObjectName = null;
				    string uploadUrl = null;
				    {
					    var task = _domain.PrepareUpload(
						    new PrepareUploadRequest()
							    .WithScope(_scope)
							    .WithAllowUserIds(_allowUserIds.ToArray())
							    .WithName(_name)
							    .WithUpdateIfExists(_updateIfExists)
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
						var task2 = task.Result.Model();
						yield return task2;

					    dataObjectName = task2.Result.Name;
					    uploadUrl = task.Result.UploadUrl;
				    }
				    {
					    var task = new UploadImplFuture(
						    _domain,
						    uploadUrl,
						    _data
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
				    }
				    {
					    var task = _domain.DataObject(
						    dataObjectName
					    ).DoneUpload(
						    new DoneUploadRequest()
					    );
					    yield return task;
					    if (task.Error != null)
					    {
						    OnError(task.Error);
						    yield break;
					    }
						var task2 = task.Result.Model();
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
		        
		        var request = UnityWebRequest.Put(result.UploadUrl, data);
		        request.downloadHandler = new DownloadHandlerBuffer();
		        await request.SendWebRequest().ToUniTask();
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
		        )).Model();
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