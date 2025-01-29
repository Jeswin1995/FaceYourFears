using UnityEngine;

public class PlayAudioOnce : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; 
    private static bool hasPlayed = false; // Static variable persists across scenes
    private const string HasPlayedKey = "HasPlayedAudio";

    private void Awake()
    {
        if (hasPlayed || PlayerPrefs.GetInt(HasPlayedKey, 0) == 1)
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        DontDestroyOnLoad(gameObject); // Keep object across scenes
        hasPlayed = true;
        PlayerPrefs.SetInt(HasPlayedKey, 1);
        PlayerPrefs.Save();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}