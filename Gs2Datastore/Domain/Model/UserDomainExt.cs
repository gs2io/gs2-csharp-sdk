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
    public partial class UserDomain
    {
        
#if UNITY_2017_1_OR_NEWER

	    public Gs2Future<DataObjectDomain> UploadFuture(
		    string scope,
		    List<string> allowUserIds,
		    byte[] data,
		    string name = null,
		    bool? updateIfExists = null
	    ) {
		    IEnumerator Impl(Gs2Future<DataObjectDomain> self) {
			    {
				    string dataObjectName;
				    string uploadUrl;
				    {
					    var future = PrepareUploadFuture(
						    new PrepareUploadByUserIdRequest()
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
					    if (future2.Error != null)
					    {
						    self.OnError(future2.Error);
						    yield break;
					    }

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
						    new DoneUploadByUserIdRequest()
					    );
					    yield return future;
					    if (future.Error != null)
					    {
						    self.OnError(future.Error);
						    yield break;
					    }
					    self.OnComplete(future.Result);
				    }
			    }
		    }

		    return new Gs2InlineFuture<DataObjectDomain>(Impl);
	    }

	#if GS2_ENABLE_UNITASK
	    public async UniTask<DataObjectDomain> UploadAsync(
		    string scope,
		    List<string> allowUserIds,
		    byte[] data,
		    string name = null,
		    bool? updateIfExists = null
	    ) {
		    string dataObjectName = null;
		    {
			    var result = await this.PrepareUploadAsync(
				    new PrepareUploadByUserIdRequest()
					    .WithScope(scope)
					    .WithAllowUserIds(allowUserIds.ToArray())
					    .WithName(name)
					    .WithUpdateIfExists(updateIfExists)
			    );
			    dataObjectName = result.DataObjectName;

			    using var request = UnityWebRequest.Put(result.UploadUrl, data);
			    request.downloadHandler = new DownloadHandlerBuffer();
			    await request.SendWebRequest();
			    if (request.responseCode != 200) {
				    throw new UnknownException(Array.Empty<RequestError>());
			    }
		    }
		    {
			    var result = await this.DataObject(
				    dataObjectName
			    ).DoneUploadAsync(
				    new DoneUploadByUserIdRequest()
			    );
			    return result;
		    }
	    }
	#endif
#endif
	        
#if !UNITY_2017_1_OR_NEWER
	    public async Task<DataObjectDomain> UploadAsync(
		    string scope,
		    List<string> allowUserIds,
		    byte[] data,
		    string name = null,
		    bool? updateIfExists = null
	    ) {
	        string dataObjectName = null;
	        {
		        var result = await this.PrepareUploadAsync(
			        new PrepareUploadByUserIdRequest()
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
			        new DoneUploadByUserIdRequest()
		        );
		        return result;
	        }
        }
#endif
    }
}