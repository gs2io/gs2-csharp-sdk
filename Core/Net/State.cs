namespace Gs2.Core.Net
{
    public enum State
    {
        Idle,               // セッションが未オープン
        Opening,            // セッションのオープン処理中
        LoggingIn,          // ログイン処理
        Available,          // セッションがオープン済み
        CancellingTasks,    // オープン済みセッションをクローズしようとして、タスクのキャンセル完了待ち
        Closing,            // セッションのクローズ処理中
        Closed,             // セッションがクローズされて、タスクのキャンセル完了待ち
    }
}