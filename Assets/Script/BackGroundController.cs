using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    private AudioSource audioSource;
    // 배경음악 클립을 저장할 배열
    public AudioClip[] musicClips;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 초기에 배경음악을 재생
        if (musicClips.Length > 0)
        {
            PlayMusic(musicClips[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 배경음악 재생 메서드
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // 배경음악 변경 메서드
    public void ChangeMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length)
        {
            PlayMusic(musicClips[index]);
        }
    }

    public void To_Forest()
    {
        ChangeMusic(1);
    }

    public void To_Vilige()
    {
        ChangeMusic(0);
    }
}
