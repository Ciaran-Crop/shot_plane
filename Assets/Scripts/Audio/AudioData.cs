using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData"), System.Serializable]
public class AudioData : ScriptableObject
{
    public AudioClip AudioClip => audioClip;
    public float Volume => volume;
    [SerializeField] AudioClip audioClip;
    [SerializeField] float volume = 1f;
}
