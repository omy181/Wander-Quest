using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    public TextMeshProUGUI textMessage;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
    }

    public void tryME(){
        textMessage.text = "YOU DID IT!";
    }
}
