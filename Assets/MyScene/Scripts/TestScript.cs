using System;
using System.Collections;
using System.Collections.Generic;
using Scenes.UX.MyScene;
using UnityEngine;

[Serializable]
public class TestScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Вкл худ");
            myUIManager.SettingsList.ForEach(x=>x.SetActive(true));
       
        }   
       else if (Input.GetKeyUp(KeyCode.K))
        {
            Debug.Log("Выкл худ");
            myUIManager.SettingsList.ForEach(x => x.SetActive(false));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            
        }
    }
}
