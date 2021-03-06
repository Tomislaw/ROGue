using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour {

    private CinemachineVirtualCamera mainCam;
    private CinemachineBasicMultiChannelPerlin camNoise;

    [SerializeField]
    private float shakeAmount = 0;

    private void Awake()
    {
        mainCam = GetComponent<CinemachineVirtualCamera>();
        camNoise = mainCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float length)
    {
        InvokeRepeating(nameof(BeginShake), 0, 0.01f);
        Invoke(nameof(StopShake), length);
    }

    private void BeginShake()
    {
        if (!(shakeAmount > 0))
        {
            return;
        }

        camNoise.m_AmplitudeGain = Random.value * shakeAmount * 2 - shakeAmount;
    }

    private void StopShake()
    {
        CancelInvoke(nameof(BeginShake));
        camNoise.m_AmplitudeGain = 0;
    }
}
