using Cysharp.Threading.Tasks;
using UnityEngine;

public class CollectiblePulse: MonoBehaviour
{
    [SerializeField] float pulseSpeed = 2f;
    [SerializeField] float pulseAmount = 0.01f;

    private Vector3 _initialScale;

    private void Start()
    {
        _initialScale = transform.localScale;
        PulseLoop().Forget();
    }

    private async UniTaskVoid PulseLoop()
    {
        while (this != null) 
        {
            float wave = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = _initialScale + Vector3.one * wave;

            await UniTask.Yield();
        }
    }
}