using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_2017_1_OR_NEWER
#if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
#endif
using UnityEngine.Networking;
#else
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
#endif
using Gs2.Core.Domain;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Net;
using Gs2.Gs2Datastore.Model;
using Gs2.Gs2Datastore.Request;

// ReSharper disable once CheckNamespace
namespace Gs2.Gs2Datastore.Domain.Model
{
    public partial class DataObjectAccessTokenDomain
    {
	    
#if UNITY_2017_1_OR_NEWER
	    public Gs2Future<byte[]> DownloadFuture() {
		    IEnumerator Impl(Gs2Future<byte[]> self) {
			    string fileUrl;
			    {
				    var task = PrepareDownloadOwnDataFuture(
					    new PrepareDownloadOwnDataRequest()
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
        public async UniTask<byte[]> DownloadAsync()
        {
	        var result = await this.PrepareDownloadOwnDataAsync(
		        new PrepareDownloadOwnDataRequest()
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
	    )
        {
	        var result = await this.PrepareDownloadOwnDataAsync(
		        new PrepareDownloadOwnDataRequest()
	        );
	        var response = await new HttpClient().GetAsync(result.FileUrl);
	        if (response.StatusCode != HttpStatusCode.OK)
	        {
		        throw new UnknownException(Array.Empty<RequestError>());
	        }
	        return await response.Content.ReadAsByteArrayAsync();
        }
#endif

#if UNITY_2017_1_OR_NEWER
	    public Gs2Future<byte[]> DownloadOwnFuture() {
		    IEnumerator Impl(Gs2Future<byte[]> self) {
			    string fileUrl;
			    {
				    var task = PrepareDownloadOwnDataFuture(
					    new PrepareDownloadOwnDataRequest()
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
        public async UniTask<byte[]> DownloadOwnAsync()
        {
	        var result = await this.PrepareDownloadOwnDataAsync(
		        new PrepareDownloadOwnDataRequest()
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
        public async Task<byte[]> DownloadOwnAsync(
	    )
        {
	        var result = await this.PrepareDownloadOwnDataAsync(
		        new PrepareDownloadOwnDataRequest()
	        );
	        var response = await new HttpClient().GetAsync(result.FileUrl);
	        if (response.StatusCode != HttpStatusCode.OK)
	        {
		        throw new UnknownException(Array.Empty<RequestError>());
	        }
	        return await response.Content.ReadAsByteArrayAsync();
        }
#endif

#if UNITY_2017_1_OR_NEWER
	    public Gs2Future<DataObjectAccessTokenDomain> ReUploadFuture(
		    byte[] data
		) {
		    IEnumerator Impl(Gs2Future<DataObjectAccessTokenDomain> self) {
			    string uploadUrl;
			    {
				    var future = PrepareReUploadFuture(
					    new PrepareReUploadRequest()
				    );
				    yield return future;
				    if (future.Error != null)
				    {
					    self.OnError(future.Error);
					    yield break;
				    }
				    uploadUrl = future.Result.UploadUrl;
			    }
			    {
				    using var request = UnityWebRequest.Put(uploadUrl, data);
				    request.downloadHandler = new DownloadHandlerBuffer();
				    yield return request.SendWebRequest();
			    }
			    {
				    var future = DoneUploadFuture(
					    new DoneUploadRequest()
				    );
				    yield return future;
				    if (future.Error != null)
				    {
					    self.OnError(future.Error);
					    yield break;
				    }
				    self.OnComplete(this);
			    }
		    }

		    return new Gs2InlineFuture<DataObjectAccessTokenDomain>(Impl);
	    }
#endif

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
        public async UniTask<DataObjectAccessTokenDomain> ReUploadAsync(
	        byte[] data
	    )
        {
	        string uploadUrl;
	        {
		        var result = await PrepareReUploadAsync(
			        new PrepareReUploadRequest()
			    );
		        uploadUrl = result.UploadUrl;
	        }
	        {
		        using var request = UnityWebRequest.Put(uploadUrl, data);
		        request.downloadHandler = new DownloadHandlerBuffer();
		        await request.SendWebRequest();
	        }
	        {
		        await DoneUploadAsync(
			        new DoneUploadRequest()
		        );
	        }
	        return this;
        }
#endif
	    
#if !UNITY_2017_1_OR_NEWER
        public async Task<DataObjectAccessTokenDomain> ReUploadAsync(
		    byte[] data
	    )
        {
	        string uploadUrl;
	        {
		        var result = await PrepareReUploadAsync(
			        new PrepareReUploadRequest()
			    );
		        uploadUrl = result.UploadUrl;
	        }
	        {
		        var response = await new HttpClient().PutAsync(uploadUrl, new ByteArrayContent(data));
		        if (response.StatusCode != HttpStatusCode.OK)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
	        }
	        {
		        await DoneUploadAsync(
			        new DoneUploadRequest()
		        );
	        }
	        return this;
        }
#endif
    }
}