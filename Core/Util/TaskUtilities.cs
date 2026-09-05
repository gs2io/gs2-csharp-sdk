using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Gs2.Core.Domain;
using Gs2.Core.Exception;
#if UNITY_2018_3_OR_NEWER
using UnityEngine.Events;
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #else // GS2_ENABLE_UNITASK
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
        #if UNITY_EDITOR
using UnityEditor;
        #endif // UNITY_EDITOR
    #endif // GS2_ENABLE_UNITASK
#endif // UNITY_2018_3_OR_NEWER

namespace Gs2.Core.Util
{
    public static class TaskUtilities
    {
        public static void Forget(this Task task)
        {
            task.ContinueWith(
                task1 => 
                {
                    if (task1.IsFaulted) Console.WriteLine(task1.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted
            );
        }

        /// <summary>
        /// サポートされた環境下では、引数に渡されたアクションをメインスレッドで実行します
        /// </summary>
        /// <param name="action">実行するアクション</param>
        /// <remarks>
        /// サポートされていない環境、あるいはメインスレッドから呼ばれた場合は直ちに実行されます。<br />
        /// 引数に渡されたアクションが、後続の処理より遅延して実行される場合があることに注意してください。<br />
        /// 現時点では Unity 2018.3 以降の環境をサポートしています。<br />
        /// </remarks>
        public static void RunOnMainThreadIfSupported(Action action)
        {
            if (action == null) return;

#if UNITY_2018_3_OR_NEWER
            if (IsOnMainThread)
            {
                action();
            }
            else
            {
    #if GS2_ENABLE_UNITASK
                UniTask.Void(async () =>
                {
                    await UniTask.SwitchToMainThread();
                    action();
                });
    #else // GS2_ENABLE_UNITASK
                _oneshotActionQueue.Enqueue(action);
    #endif // GS2_ENABLE_UNITASK
            }
#else // UNITY_2018_3_OR_NEWER
            action.Invoke();
#endif // UNITY_2018_3_OR_NEWER
        }

#if UNITY_2018_3_OR_NEWER
    #if GS2_ENABLE_UNITASK
        private static bool IsOnMainThread => PlayerLoopHelper.IsMainThread;
    #else // GS2_ENABLE_UNITASK
        private static int _mainThreadId;

        #if UNITY_2020_1_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        #else // UNITY_2020_1_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        #endif // UNITY_2020_1_OR_NEWER
        private static void Initialize()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;

            var playerloop = PlayerLoop.GetCurrentPlayerLoop();

            for (var i = 0; i < playerloop.subSystemList.Length; i++)
            {
                if (playerloop.subSystemList[i].type != typeof(Update)) continue;

                var updateFrameRandom = new PlayerLoopSystem
                {
                    type = typeof(UpdateType),
                    updateDelegate = Update,
                };

                playerloop.subSystemList[i].subSystemList = playerloop.subSystemList[i].subSystemList.Append(updateFrameRandom).ToArray();
                PlayerLoop.SetPlayerLoop(playerloop);

                break;
            }
        }

        private struct UpdateType
        {}

        private static readonly ConcurrentQueue<Action> _oneshotActionQueue = new();

        private static void Update()
        {
            while (_oneshotActionQueue.TryDequeue(out var action)) action();
        }

        private static bool IsOnMainThread => Thread.CurrentThread.ManagedThreadId == _mainThreadId;

        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void InitOnEditor()
        {
            Initialize();

            EditorApplication.update += UpdateOnEditor;
        }

        private static void UpdateOnEditor()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling && !EditorApplication.isUpdating) Update();
        }
        #endif // UNITY_EDITOR
    #endif // GS2_ENABLE_UNITASK
#endif // UNITY_2018_3_OR_NEWER

        /// <summary>
        /// 待機されたときに現在のコンテキストへ非同期的に処理を譲る、待機可能タスクを生成します。
        /// </summary>
        /// <returns>待機可能タスク</returns>
        /// <remarks>ターゲットやプリプロセッサディレクティブの値によって返り値の型が異なることに注意してください。</remarks>
#if UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK
        public static Cysharp.Threading.Tasks.YieldAwaitable Yield() => UniTask.Yield();
#else // UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK
        public static System.Runtime.CompilerServices.YieldAwaitable Yield() => Task.Yield();
#endif // UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK

#if UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK
        internal static Cysharp.Threading.Tasks.UniTask DelayAsync(int millisecondsDelay) => UniTask.Delay(millisecondsDelay);
#else
        internal static Task DelayAsync(int millisecondsDelay) => Task.Delay(millisecondsDelay);
#endif

#if UNITY_2018_3_OR_NEWER && UNITY_WEBGL && !UNITY_EDITOR
        public static async Task WaitAsync(SemaphoreSlim semaphore)
        {
            // ReSharper disable once MethodHasAsyncOverload
            while (!semaphore.Wait(0)) await Task.Yield();
        }
#else // UNITY_2018_3_OR_NEWER && UNITY_WEBGL && !UNITY_EDITOR
        public static Task WaitAsync(SemaphoreSlim semaphore) => semaphore.WaitAsync();
#endif // UNITY_2018_3_OR_NEWER && UNITY_WEBGL && !UNITY_EDITOR

        public static IEnumerator ToCoroutine(this Task task, Action callback)
        {
            var awaiter = task.GetAwaiter();
            while (!awaiter.IsCompleted) yield return null;
            awaiter.GetResult();
            callback?.Invoke();
        }

        public static IEnumerator ToCoroutine<T>(this Task<T> task, Action<AsyncResult<T>> callback)
        {
            var awaiter = task.GetAwaiter();
            while (!awaiter.IsCompleted) yield return null;
            AsyncResult<T> result;
            try
            {
                result = new AsyncResult<T>(awaiter.GetResult(), null);
            }
            catch (System.Exception exception)
            {
                result = new AsyncResult<T>(
                    default,
                    exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                );
            }
            callback?.Invoke(result);
        }

#if UNITY_2018_3_OR_NEWER
        public static IEnumerator ToCoroutine(this Task task, UnityAction callback) =>
            ToCoroutine(task, (Action)(() => callback?.Invoke()));

        public static IEnumerator ToCoroutine<T>(this Task<T> task, UnityAction<AsyncResult<T>> callback) =>
            ToCoroutine(task, (Action<AsyncResult<T>>)(result => callback?.Invoke(result)));

#if GS2_ENABLE_UNITASK
        public static IEnumerator ToCoroutine(this UniTask task, Action callback)
        {
            var awaiter = task.GetAwaiter();
            while (!awaiter.IsCompleted) yield return null;
            awaiter.GetResult();
            callback?.Invoke();
        }

        public static IEnumerator ToCoroutine<T>(this UniTask<T> task, Action<AsyncResult<T>> callback)
        {
            var awaiter = task.GetAwaiter();
            while (!awaiter.IsCompleted) yield return null;
            AsyncResult<T> result;
            try
            {
                result = new AsyncResult<T>(awaiter.GetResult(), null);
            }
            catch (System.Exception exception)
            {
                result = new AsyncResult<T>(
                    default,
                    exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                );
            }
            callback?.Invoke(result);
        }

        public static IEnumerator ToCoroutine(this UniTask task, UnityAction callback) =>
            ToCoroutine(task, (Action)(() => callback?.Invoke()));

        public static IEnumerator ToCoroutine<T>(this UniTask<T> task, UnityAction<AsyncResult<T>> callback) =>
            ToCoroutine(task, (Action<AsyncResult<T>>)(result => callback?.Invoke(result)));
#endif // GS2_ENABLE_UNITASK
#endif // UNITY_2018_3_OR_NEWER

        public static Gs2Future ToGs2Future(this Task task)
        {
            IEnumerator Impl(Gs2Future future)
            {
                var awaiter = task.GetAwaiter();
                while (!awaiter.IsCompleted) yield return null;
                try
                {
                    awaiter.GetResult();
                    future.OnComplete(null);
                }
                catch (System.Exception exception)
                {
                    future.OnError(
                        exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                    );
                }
            }

            return new Gs2InlineFuture(Impl);
        }

        public static Gs2Future<T> ToGs2Future<T>(this Task<T> task)
        {
            IEnumerator Impl(Gs2Future<T> future)
            {
                var awaiter = task.GetAwaiter();
                while (!awaiter.IsCompleted) yield return null;
                try
                {
                    future.OnComplete(awaiter.GetResult());
                }
                catch (System.Exception exception)
                {
                    future.OnError(
                        exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                    );
                }
            }

            return new Gs2InlineFuture<T>(Impl);
        }

#if UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK
        public static Gs2Future ToGs2Future(this UniTask task)
        {
            IEnumerator Impl(Gs2Future future)
            {
                var awaiter = task.GetAwaiter();
                while (!awaiter.IsCompleted) yield return null;
                try
                {
                    awaiter.GetResult();
                    future.OnComplete(null);
                }
                catch (System.Exception exception)
                {
                    future.OnError(
                        exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                    );
                }
            }

            return new Gs2InlineFuture(Impl);
        }

        public static Gs2Future<T> ToGs2Future<T>(this UniTask<T> task)
        {
            IEnumerator Impl(Gs2Future<T> future)
            {
                var awaiter = task.GetAwaiter();
                while (!awaiter.IsCompleted) yield return null;
                try
                {
                    future.OnComplete(awaiter.GetResult());
                }
                catch (System.Exception exception)
                {
                    future.OnError(
                        exception as Gs2Exception ?? new UnknownException(exception.Message, exception)
                    );
                }
            }

            return new Gs2InlineFuture<T>(Impl);
        }
#endif // UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK

#if UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK
        public static UniTask AsUniTask(this UniTask task) => task;
        public static UniTask<T> AsUniTask<T>(this UniTask<T> task) => task;
#endif // UNITY_2018_3_OR_NEWER && GS2_ENABLE_UNITASK

        public static Task AsTask(this Task task) => task;
        public static Task<T> AsTask<T>(this Task<T> task) => task;

#if UNITY_2018_3_OR_NEWER && !GS2_ENABLE_UNITASK
        public struct UnityWebRequestAsyncOperationAwaiter : ICriticalNotifyCompletion
        {
            internal UnityWebRequestAsyncOperation AsyncOperation;
            private Action _completeCallback;

            public bool IsCompleted => AsyncOperation.isDone;
            public void OnCompleted(Action callback) => UnsafeOnCompleted(callback);
            public void UnsafeOnCompleted(Action callback)
            {
                _completeCallback = callback;
                AsyncOperation.completed += HandleCompletion;
            }

            // UniTask と異なり、 UnityWebRequest がエラー状態でも例外は投げない
            public UnityWebRequest GetResult()
            {
                var webRequest = AsyncOperation.webRequest;
                AsyncOperation.completed -= HandleCompletion;
                AsyncOperation = null;
                _completeCallback = null;
                return webRequest;
            }
            
            private void HandleCompletion(AsyncOperation _) => _completeCallback?.Invoke();
        }

        public static UnityWebRequestAsyncOperationAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOperation) =>
            new() { AsyncOperation = asyncOperation };
#endif // UNITY_2018_3_OR_NEWER && !GS2_ENABLE_UNITASK
    }
}
