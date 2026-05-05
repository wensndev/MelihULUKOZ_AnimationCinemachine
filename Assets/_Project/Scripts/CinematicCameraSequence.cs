using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinematicCameraSequence : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera[] cameras;

    [Header("Sequence Settings")]
    [SerializeField] private float shotDuration = 4f;
    [SerializeField] private int activePriority = 20;
    [SerializeField] private int inactivePriority = 0;
    [SerializeField] private bool loop = true;

    private Coroutine sequenceRoutine;

    private void OnEnable()
    {
        if (cameras == null || cameras.Length == 0)
        {
            Debug.LogWarning("No Cinemachine cameras assigned.");
            return;
        }

        sequenceRoutine = StartCoroutine(PlaySequence());
    }

    private void OnDisable()
    {
        if (sequenceRoutine != null)
        {
            StopCoroutine(sequenceRoutine);
        }
    }

    private IEnumerator PlaySequence()
    {
        do
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                ActivateCamera(i);
                yield return new WaitForSeconds(shotDuration);
            }
        }
        while (loop);
    }

    private void ActivateCamera(int activeIndex)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] == null)
                continue;

            cameras[i].Priority = i == activeIndex
                ? activePriority
                : inactivePriority;
        }
    }
}