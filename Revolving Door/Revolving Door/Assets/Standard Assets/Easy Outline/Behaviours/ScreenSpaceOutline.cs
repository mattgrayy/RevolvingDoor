using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceOutline : MonoBehaviour
{
    [Header("Outline Settings")]
    [Tooltip("Should the outline be solid or fade out")]
    public bool solidOutline = false;

    [Tooltip("Strength override multiplier")]
    [Range(0, 10)]
    public float outlineStrength = 1f;

    [Tooltip("Which layers should this outline system display on")]
    public LayerMask outlineLayer;

    [Tooltip("What color should the outline be")]
    public Color m_outlineColor;

    [Tooltip("How many times should the render be downsampled")]
    [Range(0, 4)]
    public int downsampleAmount = 2;

    [Tooltip("How big should the outline be")]
    [Range(0.0f, 10.0f)]
    public float outlineSize = 1.5f;

    [Tooltip("How many times should the blur be performed")]
    [Range(1, 10)]
    public int outlineIterations = 2;

    [Space(10)]
    [Header("Component References - Do not change")]
    public Camera m_camera;
    public Canvas outlineCanvas;
    public RawImage screenOverlay;
    private RenderTexture renTexInput;
    private RenderTexture renTexRecolor;
    private RenderTexture renTexDownsample;
    private RenderTexture renTexBlur1;
    private RenderTexture renTexOut;

    private Material recolorMaterial;
    private Material blurMaterial;
    private Material outlineMaterial;

    public Shader recolorShader;
    public Shader blurShader;
    public Shader outlineShader;

    void Awake()
    {
        Init();
        outlineCanvas.gameObject.SetActive(true);

        m_camera.cullingMask = outlineLayer.value;

        outlineMaterial.SetColor("_OutlineCol", m_outlineColor);
        outlineMaterial.SetFloat("_GradientStrengthModifier", outlineStrength);
    }

    public void Init()
    {
        UpdateRenderTextureSizes();

        outlineCanvas.worldCamera = Camera.main;

        recolorMaterial = new Material(recolorShader);
        blurMaterial = new Material(blurShader);
        outlineMaterial = new Material(outlineShader);

        screenOverlay.texture = renTexOut;
        m_camera.clearFlags = CameraClearFlags.SolidColor;
        m_camera.backgroundColor = new Color(1f, 0f, 1f, 1f);
        m_camera.targetTexture = renTexInput;

        m_camera.orthographic = Camera.main.orthographic;
        m_camera.fieldOfView = Camera.main.fieldOfView;
        m_camera.farClipPlane = Camera.main.farClipPlane;
        m_camera.nearClipPlane = Camera.main.nearClipPlane;
    }

    void UpdateRenderTextureSizes()
    {
        Vector2 screenDims = ScreenDimension();
        int x = Mathf.FloorToInt(screenDims.x);
        int y = Mathf.FloorToInt(screenDims.y);
        renTexInput = new RenderTexture(x, y, 1);
        renTexDownsample = new RenderTexture(x, y, 1);
        renTexRecolor = new RenderTexture(x, y, 1);
        renTexOut = new RenderTexture(x, y, 1);
        renTexBlur1 = new RenderTexture(x, y, 1);
    }

    public Vector2 ScreenDimension()
    {
        Vector2 size = Vector2.one;
        size = new Vector2(Screen.width, Screen.height);
        return size;
    }

    void RunCalcs()
    {
        m_camera.transform.position = Camera.main.transform.position;
        m_camera.transform.rotation = Camera.main.transform.rotation;
        m_camera.Render();

        float widthMod = 1.0f / (1.0f * (1 << downsampleAmount));
        blurMaterial.SetVector("_Parameter", new Vector4(outlineSize * widthMod, -outlineSize * widthMod, 0.0f, 0.0f));

        Graphics.Blit(renTexInput, renTexRecolor, recolorMaterial);
        Graphics.Blit(renTexRecolor, renTexDownsample, blurMaterial, 0);

        outlineMaterial.SetFloat("_Solid", solidOutline ? 1f : 0f);
        outlineMaterial.SetTexture("_BlurTex", renTexDownsample);
        Graphics.Blit(renTexRecolor, renTexOut, outlineMaterial);
    }

    void LateUpdate()
    {
        RunCalcs();
    }
}
