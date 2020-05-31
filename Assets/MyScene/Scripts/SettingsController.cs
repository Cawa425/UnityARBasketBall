using UnityEngine;
using UnityEngine.UI;

namespace MyScene.Scripts
{
    public class SettingsController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            //вкл/выкл второго управления
            transform.GetComponentInChildren<Toggle>().isOn=SaveSystem._Save.playerControll;
        }
    }
}
