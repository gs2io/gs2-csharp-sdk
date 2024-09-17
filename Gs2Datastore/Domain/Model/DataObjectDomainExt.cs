using System;
using System.Collections;
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
    public partial class DataObjectDomain
    {
	    
#if UNITY_2017_1_OR_NEWER
        public Gs2Future<byte[]> DownloadFuture()
        {
	        IEnumerator Impl(Gs2Future<byte[]> self) {
		        string fileUrl;
		        {
			        var task = PrepareDownloadByUserIdAndDataObjectNameFuture(
				        new PrepareDownloadByUserIdAndDataObjectNameRequest()
			        );
			        yield return task;
			        if (task.Error != null)
			        {
				        self.OnError(task.Error);
				        yield break;
			        }

			        fileUrl = task.Result.FileUrl;
		        }
		        {
			        using var request = UnityWebRequest.Get(fileUrl);
			        request.downloadHandler = new DownloadHandlerBuffer();
			        yield return request.SendWebRequest();
			        if (request.responseCode != 200)
			        {
				        self.OnError(new UnknownException(Array.Empty<RequestError>()));
			        }
			        self.OnComplete(request.downloadHandler.data);
		        }
	        }
	        return new Gs2InlineFuture<byte[]>(Impl);
        }
		
	#if GS2_ENABLE_UNITASK
	    public async UniTask<byte[]> DownloadAsync()
	    {
		    string fileUrl;
		    {
			    var result = await PrepareDownloadByUserIdAndDataObjectNameAsync(
				    new PrepareDownloadByUserIdAndDataObjectNameRequest()
			    );
			    fileUrl = result.FileUrl;
		    }
		    {
			    using var request = UnityWebRequest.Get(fileUrl);
			    request.downloadHandler = new DownloadHandlerBuffer();
			    await request.SendWebRequest();
			    if (request.responseCode != 200)
			    {
				    throw new UnknownException(Array.Empty<RequestError>());
			    }
			    return request.downloadHandler.data;
		    }
	    }
	#endif
#endif
	    
#if !UNITY_2017_1_OR_NEWER
	    public async Task<byte[]> DownloadAsync()
	    {
		    string fileUrl;
		    {
			    var result = await PrepareDownloadByUserIdAndDataObjectNameAsync(
				    new PrepareDownloadByUserIdAndDataObjectNameRequest()
			    );
			    fileUrl = result.FileUrl;
		    }
		    {
		        var response = await new HttpClient().GetAsync(fileUrl);
		        if (response.StatusCode != HttpStatusCode.OK)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
		        return await response.Content.ReadAsByteArrayAsync();
		    }
	    }
#endif

#if UNITY_2017_1_OR_NEWER
        public Gs2Future<byte[]> DownloadByUserIdAndDataObjectNameFuture()
        {
	        IEnumerator Impl(Gs2Future<byte[]> self) {
		        string fileUrl;
		        {
			        var task = PrepareDownloadByUserIdAndDataObjectNameFuture(
				        new PrepareDownloadByUserIdAndDataObjectNameRequest()
			        );
			        yield return task;
			        if (task.Error != null)
			        {
				        self.OnError(task.Error);
				        yield break;
			        }

			        fileUrl = task.Result.FileUrl;
		        }
		        {
			        using var request = UnityWebRequest.Get(fileUrl);
			        request.downloadHandler = new DownloadHandlerBuffer();
			        yield return request.SendWebRequest();
			        if (request.responseCode != 200)
			        {
				        self.OnError(new UnknownException(Array.Empty<RequestError>()));
			        }
			        self.OnComplete(request.downloadHandler.data);
		        }
	        }
	        return new Gs2InlineFuture<byte[]>(Impl);
        }
#endif

#if UNITY_2017_1_OR_NEWER && GS2_ENABLE_UNITASK
	    public async UniTask<byte[]> DownloadByUserIdAndDataObjectNameAsync()
	    {
		    string fileUrl;
		    {
			    var result = await PrepareDownloadByUserIdAndDataObjectNameAsync(
				    new PrepareDownloadByUserIdAndDataObjectNameRequest()
			    );
			    fileUrl = result.FileUrl;
		    }
		    {
			    using var request = UnityWebRequest.Get(fileUrl);
			    request.downloadHandler = new DownloadHandlerBuffer();
			    await request.SendWebRequest();
			    if (request.responseCode != 200)
			    {
				    throw new UnknownException(Array.Empty<RequestError>());
			    }
			    return request.downloadHandler.data;
		    }
	    }
#endif
	    
#if !UNITY_2017_1_OR_NEWER
	    public async Task<byte[]> DownloadByUserIdAndDataObjectNameAsync()
	    {
		    string fileUrl;
		    {
			    var result = await PrepareDownloadByUserIdAndDataObjectNameAsync(
				    new PrepareDownloadByUserIdAndDataObjectNameRequest()
			    );
			    fileUrl = result.FileUrl;
		    }
		    {
		        var response = await new HttpClient().GetAsync(fileUrl);
		        if (response.StatusCode != HttpStatusCode.OK)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
		        return await response.Content.ReadAsByteArrayAsync();
		    }
	    }
#endif

#if UNITY_2017_1_OR_NEWER
	    
	    public Gs2Future<DataObject> ReUploadFuture(
		    byte[] data
	    ) {
		    IEnumerator Impl(Gs2Future<DataObject> self) {
			    string uploadUrl;
			    {
				    var future = PrepareReUploadFuture(
					    new PrepareReUploadByUserIdRequest()
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
					    new DoneUploadByUserIdRequest()
				    );
				    yield return future;
				    if (future.Error != null)
				    {
					    self.OnError(future.Error);
					    yield break;
				    }
				    var task2 = future.Result.ModelFuture();
				    yield return task2;
				    self.OnComplete(task2.Result);
			    }
		    }
		    return new Gs2InlineFuture<DataObject>(Impl);
	    }

	#if GS2_ENABLE_UNITASK
	    
	    public async UniTask<DataObject> ReUploadAsync(
	        byte[] data
        )
        {
	        {
		        var result = await this.PrepareReUploadAsync(
			        new PrepareReUploadByUserIdRequest()
		        );

		        using var request = UnityWebRequest.Put(result.UploadUrl, data);
		        request.downloadHandler = new DownloadHandlerBuffer();
		        await request.SendWebRequest();
		        if (request.responseCode != 200) {
			        request.Dispose();
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
	        }
	        {
		        var result = await (await this.DoneUploadAsync(
			        new DoneUploadByUserIdRequest()
		        )).ModelAsync();
		        return result;
	        }
        }
	#endif
#endif
	    
#if !UNITY_2017_1_OR_NEWER
	    
	    public async Task<DataObject> ReUploadAsync(
	        byte[] data
        )
        {
	        {
		        var result = await this.PrepareReUploadAsync(
			        new PrepareReUploadByUserIdRequest()
		        );

		        var response = await new HttpClient().PutAsync(result.UploadUrl, new ByteArrayContent(data));
		        if (response.StatusCode != HttpStatusCode.OK)
		        {
			        throw new UnknownException(Array.Empty<RequestError>());
		        }
	        }
	        {
		        var result = await (await this.DoneUploadAsync(
			        new DoneUploadByUserIdRequest()
		        )).ModelAsync();
		        return result;
	        }
        }
#endif
    }
}