using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nameText, messageText;
    public float textUpTime = 1;


    public void DisableTexts()
    {
        if(textUpTime > 0)
        {
            textUpTime -= Time.deltaTime;
        }
        else
        {
            nameText.text = " ";
            messageText.text = " ";
        }
    }
}
