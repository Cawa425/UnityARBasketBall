using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MyScene.Scripts
{
    public class SaveSystem : MonoBehaviour
    {
        public static Save _Save =new Save();
        private string _path;//путь к файлу
        private void Start()
        {
            //Файл сохранения для мобилки
            _path = Path.Combine(Application.persistentDataPath,"Save.json");

            if (File.Exists(_path))
            {
                _Save = JsonUtility.FromJson<Save>(File.ReadAllText(_path));
                Debug.Log("Сохранение успешно загружено");
            }
            else Debug.Log("Сохранение не найдено" );
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus) File.WriteAllText(_path,JsonUtility.ToJson(_Save));
        }

        private void OnApplicationQuit()
        {
            File.WriteAllText(_path,JsonUtility.ToJson(_Save));
        }

        public void ControllChanged(bool value)
        {
            _Save.playerControll = value;
        }
        
        [Serializable]
        public class Save
        {
          public bool playerControll;
        }
    }
}
