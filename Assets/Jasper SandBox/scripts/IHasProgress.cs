using System;

public interface IHasProgress
{
    event Action<float> OnProgressChanged;    // 0 tot 1
    event Action<bool> OnFoodPresenceChanged; // true = balk tonen
}
