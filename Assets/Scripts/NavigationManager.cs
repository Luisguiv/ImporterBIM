using UnityEngine;
using System.Collections;
using System.Linq;
using SimpleFileBrowser; // Simple File Browser
using Dummiesman; // OBJLoader (Dummiesman)

public class NavigationManager : MonoBehaviour
{
    public Camera cam;
    public Transform orbitPivot; // vazio na cena ou criado dinamicamente
    public float orbitSensitivity = 180f;
    public float orbitPitchMin = -85f;
    public float orbitPitchMax = 85f;

    public float flyMoveSpeed = 5f;
    public float flyBoostMultiplier = 4f;
    public float flyLookSensitivity = 180f;

    public float zoomSpeed = 5f;
    public float fovZoomSpeed = 60f;
    public float minDistance = 0.1f;
    public float maxDistance = 1000f;
    public float focusPadding = 1.2f; // multiplicador para afastar um pouco

    public KeyCode toggleModeKey = KeyCode.Tab;
    public KeyCode focusKey = KeyCode.F;
    public KeyCode resetKey = KeyCode.R;

    public enum NavMode { Orbit, Fly }
    public NavMode mode = NavMode.Orbit;

    private float orbitYaw;
    private float orbitPitch;
    private float orbitDistance = 5f;

    public Vector3 defaultCamPos;
    public Quaternion defaultCamRot;
    public float defaultFOV;

    public GameObject currentModel;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
        if (orbitPivot == null)
        {
            var go = new GameObject("OrbitPivot");
            orbitPivot = go.transform;
            orbitPivot.position = Vector3.zero;
        }

        defaultCamPos = cam.transform.position;
        defaultCamRot = cam.transform.rotation;
        defaultFOV = cam.fieldOfView;

        // Inicializa órbita com base na pose atual
        Vector3 toCam = cam.transform.position - orbitPivot.position;
        orbitDistance = toCam.magnitude;
        var look = Quaternion.LookRotation((orbitPivot.position - cam.transform.position).normalized, Vector3.up).eulerAngles;
        orbitYaw = look.y;
        orbitPitch = look.x;
    }

    void Update()
    {
        // Toggle de modo
        if (Input.GetKeyDown(toggleModeKey))
        {
            if (mode == NavMode.Orbit) EnterFly();
            else EnterOrbit();
        }

        // Reset
        if (Input.GetKeyDown(resetKey))
            ResetView();

        // Focus
        if (Input.GetKeyDown(focusKey) && currentModel != null)
            FocusObject(currentModel);

        // Input por modo
        if (mode == NavMode.Orbit)
        {
            UpdateOrbit();
            UpdateZoomDolly();
        }
        else
        {
            UpdateFly();
            UpdateFOVZoom();
        }
    }

    void EnterFly()
    {
        mode = NavMode.Fly;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void EnterOrbit()
    {
        mode = NavMode.Orbit;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateOrbit()
    {
        if (Input.GetMouseButton(2))
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            orbitYaw += mx * orbitSensitivity * Time.deltaTime;
            orbitPitch -= my * orbitSensitivity * Time.deltaTime;
            orbitPitch = Mathf.Clamp(orbitPitch, orbitPitchMin, orbitPitchMax);
        }

        Quaternion rot = Quaternion.Euler(orbitPitch, orbitYaw, 0f);
        Vector3 targetPos = orbitPivot.position - rot * Vector3.forward * orbitDistance;
        cam.transform.SetPositionAndRotation(targetPos, rot);
    }

    void UpdateZoomDolly()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 1e-4f)
        {
            // O zoomSpeed já está calibrado dinamicamente em FocusObject()
            float factor = 1f - scroll * zoomSpeed;
            orbitDistance = Mathf.Clamp(orbitDistance * factor, minDistance, maxDistance);
        }
    }

    void UpdateFly()
    {
        // Mouse look (RMB segurado ou mouse sempre travado em Fly)
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        cam.transform.rotation *= Quaternion.Euler(-my * flyLookSensitivity * Time.deltaTime,
                                                   mx * flyLookSensitivity * Time.deltaTime,
                                                   0f);

        // WASD + QE
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float upDown = 0f;
        if (Input.GetKey(KeyCode.E)) upDown += 1f;
        if (Input.GetKey(KeyCode.Q)) upDown -= 1f;

        float speed = flyMoveSpeed * (Input.GetKey(KeyCode.LeftShift) ? flyBoostMultiplier : 1f);
        Vector3 move = (cam.transform.forward * v + cam.transform.right * h + Vector3.up * upDown) * speed * Time.deltaTime;
        cam.transform.position += move;
    }

    void UpdateFOVZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 1e-4f)
        {
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView * (1f - scroll * (fovZoomSpeed / 100f)), 15f, 90f);
        }
    }

    public void ResetView()
    {
        cam.transform.SetPositionAndRotation(defaultCamPos, defaultCamRot);
        cam.fieldOfView = defaultFOV;
    }

    public void FocusObject(GameObject go)
    {
        if (go == null) return;

        var bounds = ComputeBounds(go);
        orbitPivot.position = bounds.center;

        float radius = bounds.extents.magnitude * focusPadding;
        radius = Mathf.Max(radius, 0.1f); // Garante um mínimo

        float fovRad = cam.fieldOfView * Mathf.Deg2Rad;
        float dist = Mathf.Max(minDistance, radius / Mathf.Tan(fovRad * 0.5f));

        orbitDistance = dist;

        // Ajusta dinamicamente sensibilidade e passo do scroll conforme tamanho
        zoomSpeed = Mathf.Clamp(radius * 3f, 0.01f, 2f);    // Zoom mais suave p/ objetos pequenos
        minDistance = Mathf.Max(0.01f, radius * 0.1f);
        maxDistance = Mathf.Max(10f, radius * 20f);

        cam.transform.position = orbitPivot.position - cam.transform.rotation * Vector3.forward * orbitDistance;
    }

    public Bounds ComputeBounds(GameObject root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        if (renderers.Length == 0)
            return new Bounds(root.transform.position, Vector3.zero);

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            b.Encapsulate(renderers[i].bounds);
        return b;
    }

    // Botão do Canvas chama este método
    public void OnClickImportModel()
    {
        StartCoroutine(ImportModelRoutine());
    }

    IEnumerator ImportModelRoutine()
    {
        // Agora filtra OBJ e GLTF/GLB
        FileBrowser.SetFilters(true,
            new FileBrowser.Filter("Models", ".obj", ".gltf", ".glb")
        );
        FileBrowser.SetDefaultFilter(".obj");

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Selecione modelo", "Abrir");

        if (!FileBrowser.Success || FileBrowser.Result == null || FileBrowser.Result.Length == 0)
            yield break;

        string path = FileBrowser.Result[0];

        // Destrói o modelo anterior, se houver
        if (currentModel != null) Destroy(currentModel);

        string ext = System.IO.Path.GetExtension(path).ToLowerInvariant();

        if (ext == ".obj")
        {
            // OBJ Loader comum
            currentModel = new OBJLoader().Load(path);
            // Ajusta transformações se necessário
            currentModel.transform.position = Vector3.zero;
            // Foca e salva defaults
            FocusObject(currentModel);
            defaultCamPos = cam.transform.position;
            defaultCamRot = cam.transform.rotation;
            defaultFOV = cam.fieldOfView;
            EnterOrbit();
        }
        else if (ext == ".gltf" || ext == ".glb")
        {
            // Chama seu loader de GLTF/GLB (adaptando caso seu loader esteja em outro objeto)
            // Supondo que tenha uma referência/instância para seu loader:
            var gltfLoader = Object.FindFirstObjectByType<GltfLoaderButton>();
            if (gltfLoader != null)
            {
                // Prefixa corretamente para local: "file:///"
                gltfLoader.LoadModel("file:///" + path.Replace("\\", "/"));
            }
            else
            {
                Debug.LogError("Nenhuma instância de GltfLoaderButton encontrada na cena!");
            }
            // Para glTFast, normalmente você controla ajuste/escala cá dentro do script do loader se desejar.
        }
        else
        {
            Debug.LogWarning("Extensão não suportada.");
            yield break;
        }
    }
}