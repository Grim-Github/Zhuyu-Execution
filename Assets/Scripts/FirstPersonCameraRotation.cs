using UnityEngine;

/// <summary>
/// A simple FPP (First Person Perspective) camera rotation script.
/// Like those found in most FPS (First Person Shooter) games.
/// </summary>
public class FirstPersonCameraRotation : MonoBehaviour
{

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;


    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";
    private const int V = 2;
    private const int V1 = 2;
    private const float V2 = .2f;
    [Header("Components")]
    [SerializeField] private GameObject scope;
    [SerializeField] private GameObject rifle;

    [HideInInspector] public bool enableScope = false;
    float defaultFOV;
    private float defaultSens;

    [Header("Sway")]
    [SerializeField] private float frequency;
    [SerializeField] private float amplitude;

    private void Awake()
    {
        defaultFOV = Camera.main.fieldOfView;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        defaultSens = sensitivity;
    }


    void Scope()
    {
        if (Input.GetMouseButtonDown(1))
        {
            enableScope = true;
            frequency = V;
            amplitude = V1;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            enableScope = false;
            frequency = V2;
            amplitude = V2;
        }

        if (enableScope)
        {
            scope.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV / 4, Time.deltaTime * 3);
            sensitivity = defaultSens / 4;
        }
        else
        {
            scope.SetActive(false);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaultFOV, Time.deltaTime * 3);
            sensitivity = defaultSens;
        }
    }


    void Update()
    {
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x + Mathf.Sin(frequency / 2 * Time.time) * amplitude / 2, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y + Mathf.Sin(frequency * Time.time) * amplitude, Vector3.left);
        Scope();

        transform.localRotation = xQuat * yQuat;
    }
}