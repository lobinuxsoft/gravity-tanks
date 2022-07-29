using UnityEngine;

namespace CryingOnionTools.AudioTools
{
    [RequireComponent(typeof(MusicTrigger))]
    public class RandomMusicRequest : MonoBehaviour
    {
        [SerializeField] AudioClip[] musics;

        MusicTrigger musicTrigger;

        private void Awake()
        {
            musicTrigger = GetComponent<MusicTrigger>();
        }

        private void Start()
        {
            Random.InitState(Mathf.RoundToInt(Time.time));
            musicTrigger.Music = musics[Random.Range(0, musics.Length)];
            musicTrigger.PlayMusic();
        }
    }
}
