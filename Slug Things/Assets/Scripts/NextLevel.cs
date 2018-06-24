using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;

public class NextLevel : MonoBehaviour {
    int next_scene;
	// Use this for initialization
	void Start () {
        next_scene = SceneManager.GetActiveScene().buildIndex + 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Scene curr_scene = SceneManager.GetActiveScene();
        int curr_index = curr_scene.buildIndex + 1;
        if (collision.gameObject.CompareTag("Player"))
        {
            NetworkManager.singleton.ServerChangeScene(NameFromIndex(curr_index));
        }
    }

    public void ReloadLevel()
    {
        Scene curr_scene = SceneManager.GetActiveScene();
        int curr_index = curr_scene.buildIndex;
        NetworkManager.singleton.ServerChangeScene(NameFromIndex(curr_index));
    }
}
