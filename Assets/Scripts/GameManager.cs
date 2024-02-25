using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nameText, messageText;
    [SerializeField] private AudioClip loseSound;
    public float textUpTime = 1;


    public void LoseGame()
    {
        GetComponent<AudioSource>().PlayOneShot(loseSound);
    }

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
