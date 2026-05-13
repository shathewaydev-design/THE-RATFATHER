using UnityEngine;

public class BossChargeTelegraph : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Charge Duration")]
    [SerializeField] private float chargeDuration = 3f;

    [Header("Colors")]
    [SerializeField] private Color idleColor = new Color(0.15f, 0f, 0f);
    [SerializeField] private Color buildupColor = Color.red;
    [SerializeField] private Color dangerColor = new Color(1f, 0.25f, 0.25f);
    [SerializeField] private Color flashColor = Color.white;

    [Header("Emission")]
    [SerializeField] private float minEmission = 0.25f;
    [SerializeField] private float maxEmission = 8f;

    [Header("Flash")]
    [SerializeField] private float minFlashSpeed = 8f;
    [SerializeField] private float maxFlashSpeed = 35f;

    [SerializeField] private float flashIntensityMultiplier = 2f;

    private MaterialPropertyBlock mpb;

    private float timer;
    private bool charging;

    private static readonly int EmissionColorID =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        StartCharge();
    }

    private void Update()
    {
        if (!charging)
            return;

        timer += Time.deltaTime;

        float chargePercent = timer / chargeDuration;
        chargePercent = Mathf.Clamp01(chargePercent);

        UpdateChargeVisual(chargePercent);

        if (chargePercent >= 1f)
        {
            charging = false;

            // Optional:
            // disable after fully charged
            // gameObject.SetActive(false);
        }
    }

    public void StartCharge()
    {
        timer = 0f;
        charging = true;
    }

    private void UpdateChargeVisual(float chargePercent)
    {
        Color finalColor;
        float emission;

        // ------------------------------------------------
        // 0% -> 60%
        // subtle buildup
        // ------------------------------------------------
        if (chargePercent < 0.6f)
        {
            float t = chargePercent / 0.6f;

            t = Mathf.SmoothStep(0f, 1f, t);

            finalColor = Color.Lerp(
                idleColor,
                buildupColor,
                t
            );

            emission = Mathf.Lerp(
                minEmission,
                2f,
                t
            );
        }

        // ------------------------------------------------
        // 60% -> 85%
        // rapid intensification
        // ------------------------------------------------
        else if (chargePercent < 0.85f)
        {
            float t = Mathf.InverseLerp(
                0.6f,
                0.85f,
                chargePercent
            );

            // sharper ramp
            t = t * t;

            finalColor = Color.Lerp(
                buildupColor,
                dangerColor,
                t
            );

            emission = Mathf.Lerp(
                2f,
                maxEmission,
                t
            );
        }

        // ------------------------------------------------
        // 85% -> 100%
        // aggressive flashing/strobing
        // ------------------------------------------------
        else
        {
            float t = Mathf.InverseLerp(
                0.85f,
                1f,
                chargePercent
            );

            // Flash speed accelerates toward impact
            float flashSpeed = Mathf.Lerp(
                minFlashSpeed,
                maxFlashSpeed,
                t
            );

            // Sin pulse
            float flash = Mathf.Sin(Time.time * flashSpeed);

            // Convert -1/1 to 0/1
            flash = flash * 0.5f + 0.5f;

            // Sharper flashes
            flash = Mathf.Pow(flash, 4f);

            finalColor = Color.Lerp(
                dangerColor,
                flashColor,
                flash
            );

            emission = Mathf.Lerp(
                maxEmission,
                maxEmission * flashIntensityMultiplier,
                flash
            );

            // Final escalation
            emission *= Mathf.Lerp(
                1f,
                1.75f,
                t
            );
        }

        ApplyEmission(finalColor, emission);
    }

    private void ApplyEmission(Color color, float intensity)
    {
        targetRenderer.GetPropertyBlock(mpb);

        mpb.SetColor(
            EmissionColorID,
            color * intensity
        );

        targetRenderer.SetPropertyBlock(mpb);
    }
}