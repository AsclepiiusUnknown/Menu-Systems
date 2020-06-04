using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player player;
    private LivingEntity playerEntity;
    private MapGenerator mapGenerator;
    public SaveAndLoad saveAndLoad;

    private void Awake()
    {
        if(saveAndLoad != null)
        {
            //saveAndLoad.Load();
        }
    }

    /*private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() != null && player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetComponent<LivingEntity>() && player == null)
        {
            playerEntity = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetComponent<LivingEntity>();
        }

        if (GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>() != null && mapGenerator == null)
        {
            mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        }
    }

    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>() && player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }*/
}
