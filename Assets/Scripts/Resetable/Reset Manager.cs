using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public static ResetManager Instance { get; private set; }

    private readonly List<IResettable> resettables = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Register(IResettable resettable)
    {
        if (!resettables.Contains(resettable))
            resettables.Add(resettable);
    }

    public void Unregister(IResettable resettable)
    {
        resettables.Remove(resettable);
    }

    public void ResetAll()
    {
        foreach (var r in resettables)
        {
            r.ResetState();
        }
    }
}
