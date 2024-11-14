using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
    private async void Awake()
    {
        try
        {
            await DoAwaitAsync();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Awaitable DoAwaitAsync()
    {
        await Awaitable.WaitForSecondsAsync(1, destroyCancellationToken);
        Debug.Log("That message won't be logged.");
    }

    private void Start()
    {
        Destroy(this);
    }
}