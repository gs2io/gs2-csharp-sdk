using System;
using System.Threading.Tasks;
using Gs2.Core.Model;

namespace Gs2.Core.Net.Chaos
{
    public class ChaosGs2RestSession : Gs2RestSession
    {
        private readonly float _chaos;
        private readonly Random _random;
        private readonly object _randomLock = new object();

        public ChaosGs2RestSession(IGs2Credential basicGs2Credential, float chaos, Region region = Region.ApNortheast1, bool checkCertificateRevocation = true, string steadyEndpoint = null) : base(basicGs2Credential, region, checkCertificateRevocation, steadyEndpoint) {
            this._chaos = chaos;
            this._random = new Random();
        }

        public ChaosGs2RestSession(IGs2Credential basicGs2Credential, string region, float chaos, bool checkCertificateRevocation = true, string steadyEndpoint = null) : base(basicGs2Credential, region, checkCertificateRevocation, steadyEndpoint) {
            this._chaos = chaos;
            this._random = new Random();
        }

        private RestResult CreateNeedRetryResult() {
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
            return new RestResult(
                exception.StatusCode,
                exception.Message
            );
        }

        protected override Task<RestResult> InvokeRequestAsync(RestSessionRequest request)
        {
            lock (_randomLock)
            {
                if (this._random.NextDouble() < this._chaos)
                {
                    return Task.FromResult(CreateNeedRetryResult());
                }
            }
            return base.InvokeRequestAsync(request);
        }

    }
}
