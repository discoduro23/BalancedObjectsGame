using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CanvasController : MonoBehaviour
{

    public TextMeshProUGUI version = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if (version != null)    version.text = "Version: " + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
