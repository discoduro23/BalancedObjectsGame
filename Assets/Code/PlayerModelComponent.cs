using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerModelComponent : MonoBehaviour
{
    public MeshFilter playerMF = null;
    public Mesh[] playerMeshes = null;

    private int meshLenght = 0;

    // Start is called before the first frame update
    void Start()
    {
        meshLenght = playerMeshes.Length;

        playerMF.mesh = playerMeshes[PlayerPrefs.GetInt("PlayerSkin")];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextSkin()
    {
        int currentSkin = PlayerPrefs.GetInt("PlayerSkin");

        currentSkin++;
        if (currentSkin >= meshLenght)
        {
            PlayerPrefs.SetInt("PlayerSkin", 0);
            playerMF.mesh = playerMeshes[0];
        }
        else
        {
            PlayerPrefs.SetInt("PlayerSkin", currentSkin);
            playerMF.mesh = playerMeshes[currentSkin];
        }
    }
}
