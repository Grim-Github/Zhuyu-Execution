using Lexone.UnityTwitchChat;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class TwitchPepeSpawner : MonoBehaviour
{
    public Chatter chatterObject;
    [SerializeField] public List<string> Spawnedchatters = new List<string>();
    [SerializeField][Range(0, 100)] private float spawnChance = 30;
    [SerializeField][Range(0, 25)] private int maxPepes = 20;
    [SerializeField] TwitchMessageEventExecutor executor;
    [SerializeField] private GameObject[] pepe;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private string[] stringToCheck;

    private void Start()
    {

        IRC.Instance.OnChatMessage += OnChatMessage;
    }

    private void OnChatMessage(Chatter chatter)
    {
       // Debug.Log($"<color=#fef83e><b>[CHAT LISTENER]</b></color> New chat message from {chatter.tags.displayName}");
        executor.GetComponent<TwitchMessageEventExecutor>();
        GameObject[] allPepes = GameObject.FindGameObjectsWithTag("Pepe");

        executor.TryToParseMessageOnEvents(chatter.message.ToLower());

        if(allPepes.Length >= maxPepes)
        {
            return;
        }

        if (!Spawnedchatters.Contains(chatter.tags.displayName))
        {
            if ((Random.Range(0, 100) <= spawnChance) || stringToCheck.Any(word => chatter.message.ToLower().Contains(word.ToLower())))
            {
                GameObject recentlySpawnedChatter;

                if (chatter.message.ToLower().Contains("baseg"))
                {
                    recentlySpawnedChatter = Instantiate(pepe[1], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                }
                else
                {
                    recentlySpawnedChatter = Instantiate(pepe[0], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

                }
                Spawnedchatters.Add(chatter.tags.displayName);

                TextMeshPro chatterNamePlate = recentlySpawnedChatter.GetComponentInChildren<TextMeshPro>();

                chatterNamePlate.text = chatter.tags.displayName;
                chatterNamePlate.color = chatter.GetNameColor();
                recentlySpawnedChatter.GetComponent<Pepe>().chatter = chatter;
                recentlySpawnedChatter.transform.name = chatter.tags.displayName;


                if (stringToCheck.Any(word => chatter.message.ToLower().Contains(word.ToLower())))
                {
                    recentlySpawnedChatter.GetComponent<Pepe>().isBad = true;
                }
                Debug.Log($"<color=#b87333><b>[CHAT LISTENER]</b></color> Succesfully spawned {chatter.tags.displayName}");
            }
        }
        else
        {
            GameObject finding = GameObject.Find(chatter.tags.displayName);

            Debug.Log("Found Bozo");
            if (finding != null)
            {

                finding.GetComponent<Pepe>().chatter.message = chatter.message;
                if (stringToCheck.Any(word => chatter.message.ToLower().Contains(word.ToLower())))
                {
                    Debug.Log($"<color=#ff0000><b>[CHAT LISTENER]</b></color> New bad actor: " + chatter.tags.displayName);
                    finding.GetComponent<Pepe>().isBad = true;
                }

            }

        }
    }
}