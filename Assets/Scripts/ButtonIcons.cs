using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIcons : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;

    //[Button]
    //public void SavePng()
    //{
    //    Texture2D _vloume;
    //    _vloume = SdfIcons.CreateTransparentIconTexture(SdfIconType.VolumeMuteFill, Color.white, 120, 120, 0);
    //    byte[] playIconData = _vloume.EncodeToPNG();
    //    File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "mute.png"), playIconData);
    //    Debug.Log(Application.persistentDataPath);
    //    Debug.Log("SavePng: " + Path.Combine(Application.persistentDataPath, "vloume.png"));
    //}
}