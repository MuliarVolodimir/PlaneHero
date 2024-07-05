using System.Collections.Generic;
using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField] List<EconomicResource> _resources;
    [SerializeField] List<Item> _planes;

    private void Start()
    {
        var appData = ApplicationData.Instance;
        appData.InitResources(_resources);
        appData.InitPlanes(_planes);
        appData.LoadData();

        DontDestroyOnLoad(gameObject);
        SceneLoader.Instance?.LoadScene(SceneLoader.Scene.MainScene);
    }

    private void OnApplicationQuit()
    {
        ApplicationData.Instance.SaveData();
    }
}
