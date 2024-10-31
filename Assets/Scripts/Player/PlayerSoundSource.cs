using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SEType
{
    Asiba_put,
    Attack,
    AttackHit,
    Damaged,
    Jump,
}
public class PlayerSoundSource : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources;

    private Dictionary<SEType, AudioSource> audioSourceDictionary;
    private void Awake()
    {
        audioSourceDictionary = new Dictionary<SEType, AudioSource>();
        foreach(AudioSource audioSource in audioSources)
        {
            ClipnameToSEType(audioSource);
        }
    }

    private void ClipnameToSEType(AudioSource audioSource)
    {
        string clipName = audioSource.clip.name;
        if(clipName == "Asiba_Put")
        {
            audioSourceDictionary.Add(SEType.Asiba_put, audioSource); 
        }else if(clipName == "CAC_Attack")
        {
            audioSourceDictionary.Add(SEType.Attack, audioSource);
        }else if(clipName == "CAC_Damaged")
        {
            audioSourceDictionary.Add(SEType.Damaged, audioSource);
        }else if(clipName == "CAC_Jump")
        {
            audioSourceDictionary.Add(SEType.Jump, audioSource);
        }else if(clipName == "AttackHit")
        {
            audioSourceDictionary.Add(SEType.AttackHit, audioSource);
        }
        else
        {
            Debug.Log("そのような音声ファイルは想定していません");
        }
    }

    public void PlaySound(SEType type)
    {
        AudioSource playSoundSource = null;
        switch (type)
        {
            case SEType.Asiba_put:
                playSoundSource = audioSourceDictionary[SEType.Asiba_put];
                break;

            case SEType.Attack:
                playSoundSource = audioSourceDictionary[SEType.Attack];
                break;

            case SEType.Damaged:
                playSoundSource = audioSourceDictionary[SEType.Damaged];
                break;

            case SEType.Jump:
                playSoundSource = audioSourceDictionary[SEType.Jump];
                break;

            case SEType.AttackHit:
                playSoundSource = audioSourceDictionary[SEType.AttackHit];
                break;

            default:
                break;
        }

        if(playSoundSource != null)
        {
            playSoundSource.Play();
        }
    }
}
