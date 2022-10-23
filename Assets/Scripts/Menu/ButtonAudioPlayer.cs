using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioPlayer : MonoBehaviour
{

    public AudioClip[] _clickSound = new AudioClip[2];
    public void PlayClickSound()
    {
        GameManager.Instance._audioManager.PlayASound(_clickSound[Random.Range(0,2)], false, false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
