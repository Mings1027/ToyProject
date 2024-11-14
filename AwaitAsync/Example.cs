using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Example : MonoBehaviour
{
    private CancellationTokenSource _tokenSource;

    private async void Awake()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            Debug.Log("This message is logged every second.");
            await Task.Delay(1000, destroyCancellationToken);
        }
    }

    private async void OnEnable()
    {
        _tokenSource = new CancellationTokenSource();
        try
        {
            await DoAwaitAsync(_tokenSource.Token);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void OnDisable()
    {
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }

    private static async Awaitable DoAwaitAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await Awaitable.WaitForSecondsAsync(1, token);
            Debug.Log("This message is logged every second.");
        }
    }
}

public static class Boot
{
    // https://mentum.tistory.com/680
    // 아래 어트리뷰트 붙이면 게임 시작할 때 호출됨 static이라서 씬에 존재안해도 호출됨 Mono아닌 클래스도 됨 static 메소드만 됨
    // [RuntimeInitializeOnLoadMethod] 
    public static async Awaitable LogAsync()
    {
        var cancellationToken = Application.exitCancellationToken;
        while (!cancellationToken.IsCancellationRequested)
        {
            Debug.Log("This message is logged every second.");
            await Task.Delay(1000, cancellationToken);
        }
    }
}