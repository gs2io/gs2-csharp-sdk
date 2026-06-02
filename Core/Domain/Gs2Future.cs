using System;
using System.Collections;
using Gs2.Core.Exception;
using Gs2.Core.Model;
using Gs2.Core.Net;

namespace Gs2.Core.Domain
{
    public abstract class Gs2Future : Gs2Future<object>
    {
    }

    public abstract class Gs2Future<TResult> : IFuture<TResult> 
    {
        public TResult Result { get; private set; }
        public Gs2Exception Error { get; private set; }
        private IEnumerator InflightAction { get; set; }

        protected abstract IEnumerator Action();

        public bool IsComplete()
        {
            return Result != null || Error != null;
        }

        public void OnError(Gs2Exception error)
        {
            Error = error;
        }
        
        public void OnComplete(TResult result)
        {
            Result = result;
        }

        public bool MoveNext()
        {
            if (InflightAction == null)
            {
                try
                {
                    InflightAction = Action();
                }
                catch (NotImplementedException)
                {
                
                }
            }
            return InflightAction?.MoveNext() ?? false;
        }

        public void Reset()
        {
            if (InflightAction == null)
            {
                try
                {
                    InflightAction = Action();
                }
                catch (NotImplementedException)
                {
                
                }
            }
            InflightAction?.Reset();
        }

        public object Current => InflightAction?.Current;
    }

    public class Gs2InlineFuture : Gs2Future
    {
        private readonly Func<Gs2Future, IEnumerator> _func;
        
        public Gs2InlineFuture(
            Func<Gs2Future, IEnumerator> func
        )
        {
            _func = func;
        }
        
        protected override IEnumerator Action()
        {
            return _func.Invoke(this);
        }
    }

    public class Gs2InlineFuture<TResult> : Gs2Future<TResult>
    {
        private readonly Func<Gs2Future<TResult>, IEnumerator> _func;
        
        public Gs2InlineFuture(
            Func<Gs2Future<TResult>, IEnumerator> func
        )
        {
            _func = func;
        }
        
        protected override IEnumerator Action()
        {
            return _func.Invoke(this);
        }
    }
}