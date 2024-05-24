using UnityEngine;

public class AudioManagerInitializer : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    void Awake()
    {
        if (SoundManager.instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}
