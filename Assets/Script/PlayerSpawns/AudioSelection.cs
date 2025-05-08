using UnityEngine;

public class MapMusicPlayer : MonoBehaviour
{
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;

    private void Start()
    {
        int mapIndex = PlayerPrefs.GetInt("SelectedMap", 1);
        AudioSource audioSource = GetComponent<AudioSource>();

        switch (mapIndex)
        {
            case 1: audioSource.clip = music1; break;
            case 2: audioSource.clip = music2; break;
            case 3: audioSource.clip = music3; break;
        }

        audioSource.loop = true;
        audioSource.Play();
    }
}
