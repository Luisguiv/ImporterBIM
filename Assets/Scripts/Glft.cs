using UnityEngine;
using GLTFast;
using System.Threading.Tasks;
using UnityEngine.UI;

public class GltfLoaderButton : MonoBehaviour
{

    public NavigationManager navManager; // seta via Inspector ou via script

    public async void LoadModel(string filePath)
    {
        var gltf = new GltfImport();
        bool success = await gltf.Load(filePath);
        if (success)
        {
            GameObject go = new GameObject("GLTF_Model");
            await gltf.InstantiateMainSceneAsync(go.transform);

            if (navManager != null)
            {
                navManager.currentModel = go;         // Atualiza referência para navegação
                navManager.FocusObject(go);           // Foca imediatamente no novo objeto
                navManager.EnterOrbit();              // Garante modo orbital após importação GLB
                navManager.defaultCamPos = navManager.cam.transform.position; // Atualiza defaults
                navManager.defaultCamRot = navManager.cam.transform.rotation;
                navManager.defaultFOV = navManager.cam.fieldOfView;
            }
        }
        else
        {
            Debug.LogError("Falha ao carregar modelo GLTF.");
        }
    }

}
