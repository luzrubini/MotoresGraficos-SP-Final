using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorState : MonoBehaviour
{
    [Header("Etapas de grietas (intacto → roto)")]
    public Material[] crackStages; // arrastrá aquí los materiales
    [Header("Audio")]
    public AudioClip breakSound; // sonido de rotura
    public float breakVolume = 1f;

    private Renderer rend;
    private int currentStage = 0;
    private AudioSource audioSource;

    void Start()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        if (crackStages.Length > 0)
            rend.material = crackStages[0]; // espejo intacto al inicio
    }

    public void AddCrack()
    {
        if (currentStage < crackStages.Length - 1)
        {
            currentStage++;
            rend.material = crackStages[currentStage];

            // 🎧 Reproducir sonido si se rompió
            if (breakSound != null)
                audioSource.PlayOneShot(breakSound, breakVolume);
        }
    }

    public void ResetMirror()
    {
        currentStage = 0;
        rend.material = crackStages[0];
    }
}
