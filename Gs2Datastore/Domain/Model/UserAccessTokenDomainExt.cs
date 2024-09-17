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
	    public Gs2Future<DataObject> UploadFuture(
		    string scope,
		    List<string> allowUserIds,
		    byte[] data,
		    string name = null,
		    bool? updateIfExists = null
	    ) {
		    IEnumerator Impl(Gs2Future<DataObject> self) {
				string dataObjectName;
			    string uploadUrl;
			    {
				    var future = PrepareUploadFuture(
					    new PrepareUploadRequest()
						    .WithScope(scope)
						    .WithAllowUserIds(allowUserIds.ToArray())
						    .WithName(name)
						    .WithUpdateIfExists(updateIfExists)
				    );
				    yield return future;
				    if (future.Error != null)
				    {
					    self.OnError(future.Error);
					    yield break;
				    }
					var future2 = future.Result.ModelFuture();
					yield return future2;

				    dataObjectName = future2.Result.Name;
				    uploadUrl = future.Result.UploadUrl;
			    }
			    {
				    using var request = UnityWebRequest.Put(uploadUrl, data);
				    request.downloadHandler = new DownloadHandlerBuffer();
				    yield return request.SendWebRequest();
			    }
			    {
				    var future = DataObject(
					    dataObjectName
				    ).DoneUploadFuture(
					    new DoneUploadRequest()
				    );
				    yield return future;
				    if (future.Error != null)
				    {
					    self.OnError(future.Error);
					    yield break;
				    }
				    var future2 = future.Result.ModelFuture();
				    yield return future2;
				    self.OnComplete(future2.Result);
			    }
		    }

		    return new Gs2InlineFuture<DataObject>(Impl);
	    }
	    
	#if GS2_ENABLE_UNITASK
	    public async UniTask<DataObject> UploadAsync(
	        string scope,
	        List<string> allowUserIds,
	        byte[] data,
	        string name=null,
	        bool? updateIfExists=null
        )
        {
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
		        var result = await (await DataObject(
			        dataObjectName
			    ).DoneUploadAsync(
			        new DoneUploadRequest()
		        )).ModelAsync();
		        return result;
	        }
        }
	#endif
#endif
	    
#if !UNITY_2017_1_OR_NEWER
	    public async Task<DataObject> UploadAsync(
	        string scope,
	        List<string> allowUserIds,
	        byte[] data,
	        string name=null,
	        bool? updateIfExists=null
        )
        {
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
		        var result = await (await this.DataObject(
			        dataObjectName
			    ).DoneUploadAsync(
			        new DoneUploadRequest()
		        )).ModelAsync();
		        return result;
	        }
        }
#endif
	    
#if UNITY_2017_1_OR_NEWER
	    public Gs2Future<byte[]> DownloadFuture(
		    string dataObjectId
		)
	    {
		    IEnumerator Impl(Gs2Future<byte[]> self) {
			    string fileUrl;
			    {
				    var task = PrepareDownloadFuture(
					    new PrepareDownloadRequest()
			        		.WithDataObjectId(dataObjectId)
				    );
				    yield return task;
				    if (task.Error != null) {
					    self.OnError(task.Error);
					    yield break;
				    }

				    fileUrl = task.Result.FileUrl;
			    }
			    {
				    using var request = UnityWebRequest.Get(fileUrl);
				    request.downloadHandler = new DownloadHandlerBuffer();
				    yield return request.SendWebRequest();
				    if (request.responseCode != 200) {
					    self.OnError(new UnknownException(Array.Empty<RequestError>()));
					    yield break;
				    }
				    self.OnComplete(request.downloadHandler.data);
			    }
		    }

		    return new Gs2InlineFuture<byte[]>(Impl);
	    }
#endif

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<byte[]> DownloadAsync(
	        string dataObjectId
	    )
        {
	        var result = await this.PrepareDownloadAsync(
		        new PrepareDownloadRequest()
			        .WithDataObjectId(dataObjectId)
	        );
	        using var request = UnityWebRequest.Get(result.FileUrl);
	        request.downloadHandler = new DownloadHandlerBuffer();
	        await request.SendWebRequest();
	        if (request.responseCode != 200) {
		        throw new UnknownException(Array.Empty<RequestError>());
	        }
	        return request.downloadHandler.data;
        }
#endif
	    
#if !UNITY_2017_1_OR_NEWER
        public async Task<byte[]> DownloadAsync(
	        string dataObjectId
	    )
        {
	        var result = await this.PrepareDownloadAsync(
		        new PrepareDownloadRequest()
			        .WithDataObjectId(dataObjectId)
	        );
	        var response = await new HttpClient().GetAsync(result.FileUrl);
	        if (response.StatusCode != HttpStatusCode.OK)
	        {
		        throw new UnknownException(Array.Empty<RequestError>());
	        }
	        return await response.Content.ReadAsByteArrayAsync();
        }
#endif
    }
}