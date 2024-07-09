using System;
using System.Collections;
using Gs2.Core.Model;
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
using Cysharp.Threading.Tasks;
    #else
using System.Threading.Tasks;
    #endif
#endif
using System.Threading.Tasks;

namespace Gs2.Core.Net.Chaos
{
    public class ChaosGs2RestSession : Gs2RestSession
    {
        private readonly float _chaos;
        private readonly Random _random;

        public ChaosGs2RestSession(IGs2Credential basicGs2Credential, float chaos, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true) : base(basicGs2Credential, region, checkCertificateRevocation) {
            this._chaos = chaos;
            this._random = new Random();
        }

        public ChaosGs2RestSession(IGs2Credential basicGs2Credential, string region, float chaos, bool checkCertificateRevocation = true) : base(basicGs2Credential, region, checkCertificateRevocation) {
            this._chaos = chaos;
            this._random = new Random();
        }

        private void ThrowNeedRetryException(IGs2SessionRequest request) {
            var payload = new[] {
                new RequestError("chaos", "chaos.chaos.chaos.error.chaos")
            };
            var exceptions = new Gs2.Core.Exception.Gs2Exception[] {
                new Gs2.Core.Exception.QuotaLimitExceededException(payload),
                new Gs2.Core.Exception.ConflictException(payload),
                new Gs2.Core.Exception.InternalServerErrorException(payload),
                new Gs2.Core.Exception.BadGatewayException(payload),
                new Gs2.Core.Exception.ServiceUnavailableException(payload),
                new Gs2.Core.Exception.RequestTimeoutException(payload),
            };
            var exception = exceptions[this._random.Next(exceptions.Length)];
            this._result[request.TaskId] = new RestResult(
                exception.StatusCode,
                exception.Message
            );
        }
        
        // Send
        
        public override IEnumerator Send(IGs2SessionRequest request) {
            var chaos = this._random.NextDouble();
            if (chaos < this._chaos) {
                ThrowNeedRetryException(request);
                yield break;
            }
            yield return base.Send(request);
        }
        
#if !UNITY_2017_1_OR_NEWER || GS2_ENABLE_UNITASK
    #if UNITY_2017_1_OR_NEWER
        public override UniTask SendAsync(IGs2SessionRequest request)
    #else
        public override Task SendAsync(IGs2SessionRequest request)
    #endif
        {
            if (this._random.NextDouble() < this._chaos) {
                ThrowNeedRetryException(request);
    #if UNITY_2017_1_OR_NEWER
                return UniTask.CompletedTask;
    #else
                return Task.CompletedTask;
    #endif
            }
            return base.SendAsync(request);
        }
#endif

    }
}
