using System;
using System.Threading.Tasks;
#if UNITY_2018_3_OR_NEWER
    #if GS2_ENABLE_UNITASK
using Cysharp.Threading.Tasks;
    #else // GS2_ENABLE_UNITASK
using System.Collections.Concurrent;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
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
    }
}
