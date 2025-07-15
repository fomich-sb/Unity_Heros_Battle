using System;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    [HideInInspector] public event Action<int> NewAnimationStart;
    [HideInInspector] public event Action<string> PlaySound;

    public void OnAnimationStart(int id)
    {
        NewAnimationStart?.Invoke(id);
    }
    public void NeedPlaySound(string type)
    {
        PlaySound?.Invoke(type);
    }
}