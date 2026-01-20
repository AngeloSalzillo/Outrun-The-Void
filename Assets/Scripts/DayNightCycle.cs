using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material Skybox;
    Material runtimeSkybox;

    // Sunrise
    Color[] SunriseColors = new Color[] {
                             new Color(222f/255f, 137f/255f, 0f, 1f),
                             new Color(222f/255f, 137f/255f, 0f, 1f),
                             new Color(211f/255f, 46f/255f, 125f/255f, 1f)}; 
    
    // Day
    Color[] DayColors = new Color[] {
                       new Color(50f/255f, 128f/255f, 107f/255f, 1f),
                       new Color(50f/255f, 128f/255f, 107f/255f, 1f),
                       new Color(124f/255f, 215f/255f, 156f/255f, 1f)};

    // Sunset
    Color[] SunsetColors = new Color[] {
                            new Color(128f/255F, 50f/255f, 102f/255f, 1f),
                            new Color(128f/255f, 50f/255f, 102f/255f, 1f),
                            new Color(211f/255f, 101f/255f, 46f/255f, 1f)};

    // Night
    Color[] NightColors = new Color[] {
                           new Color(0f, 0f, 0f, 1f),
                           new Color(0f, 0f, 0f, 1f),
                           new Color(25f/255f, 24f/255f, 38f/255f, 1f)};

    Color SunriseCloud = new Color(79f/255f, 90f/255f, 86f/255f, 1f);
    Color DayCloud = new Color(124f/255f, 128f/255f, 127f/255f, 1f);
    Color SunsetCloud = new Color(141f/255f, 75f/255f, 67f/255f, 1f);
    Color NightCloud = new Color(41f/255f, 46f/255f, 44f/255f, 1f);


    
    float t = 0f;
    float cycleDuration = 40f; // Duration of a full day-night cycle in seconds
    float topExponent = 0.4f;
    float bottomExponent = 0.4f;
    float cloudsAngle;
    float sunSpeed = 180f / 40f;   // degrees/sec (full cycle in 40s)
    float moonSpeed = 180f / 40f;
    float sunVerAngle = 0f;
    float moonVerAngle = 0f;
    float sunAngle = -105f;
    float moonAngle = -17f;
    float offset;
    float divider;



    void Start()
    {
        runtimeSkybox = new Material(Skybox);
        RenderSettings.skybox = runtimeSkybox;
        runtimeSkybox.DisableKeyword("SUN_OFF");
        runtimeSkybox.DisableKeyword("MOON_OFF");
        runtimeSkybox.DisableKeyword("CLOUDS_OFF");
        runtimeSkybox.SetFloat("_MoonSize", 0.2f);
        runtimeSkybox.SetFloat("_SunSize", 0.2f);

        runtimeSkybox.SetMatrix(
        "sunMatrix",
        Matrix4x4.TRS(Vector3.zero,
                      Quaternion.LookRotation(Vector3.left),
                      Vector3.one)
        );

        runtimeSkybox.SetMatrix(
        "moonMatrix",
        Matrix4x4.TRS(Vector3.zero,
                      Quaternion.LookRotation(Vector3.right),
                      Vector3.one)
        );
        
        runtimeSkybox.SetFloat("_MoonHalo", 1f);

        cloudsAngle = 0f;
        runtimeSkybox.SetColor("_CloudsTint", NightCloud);
    }

    void Update()
    {
        Color[] fromSky;
        Color[] toSky;
        Color fromCloud;
        Color toCloud;


        if(t <= 0.2f){
            //Night -> Sunrise
            fromSky = NightColors;
            toSky = SunriseColors;
            fromCloud = NightCloud;
            toCloud = SunriseCloud;

            topExponent = 0.4f;
            bottomExponent = 0.4f;
            offset = 0;
            divider = 0.2f;          
        }
        else if(t > 0.2f && t < 0.35f ){
            //Sunrise -> Day
            fromSky = SunriseColors;
            toSky = DayColors;
            fromCloud = SunriseCloud;
            toCloud = DayCloud;

            topExponent = 0.4f;
            bottomExponent = 0.4f;
            offset = 0.2f;
            divider = 0.15f;

            float extinction = Mathf.Lerp(10, 0, (t - offset)/divider);
            runtimeSkybox.SetFloat("_StarsExtinction", extinction);
        }
        else if(t >= 0.35f && t < 0.5f)
        {
            //Day
            fromSky = DayColors;
            toSky = DayColors;
            fromCloud = DayCloud;
            toCloud = DayCloud;

            topExponent = 0.4f;
            bottomExponent = 0.4f;
            offset = 0.35f;
            divider = 0.15f;
        }
        else if(t >= 0.5f && t < 0.6f){
            //Day -> Sunset
            fromSky = DayColors;
            toSky = SunsetColors;
            fromCloud = DayCloud;
            toCloud = SunsetCloud;

            topExponent = 0.4f;
            bottomExponent = 0.4f;
            offset = 0.5f;
            divider = 0.1f;
        }
        else if(t >= 0.6f && t < 0.7f){
            //Sunset -> Night
            fromSky = SunsetColors;
            toSky = NightColors;
            fromCloud = SunsetCloud;
            toCloud = NightCloud;

            bottomExponent = 0.4f;
            topExponent = 0.4f;
            offset = 0.6f;
            divider = 0.1f;

            float extinction = Mathf.Lerp(0, 10, (t - offset)/divider);
            runtimeSkybox.SetFloat("_StarsExtinction", extinction);
        }
        else
        {
            //Night
            fromSky = NightColors;
            toSky = NightColors;
            fromCloud = NightCloud;
            toCloud = NightCloud;

            bottomExponent = 0.4f;
            topExponent = 0.4f;
            offset = 0.7f;
            divider = 0.3f;
        }


        if(t >= 1f)
            t = 0f;

        t += Time.deltaTime/cycleDuration;
        cloudsAngle += Time.deltaTime * 1f; // degrees/sec
        cloudsAngle %= 360f; 
        changeSkyColor(fromSky, toSky);
        changeCloudsColor(fromCloud, toCloud);
        rotateClouds(cloudsAngle);

        sunAngle -= sunSpeed * Time.deltaTime; 
        if (sunAngle <= -285f) 
            sunAngle = -105f;
        moonAngle -= moonSpeed * Time.deltaTime; 
        if (moonAngle <= -220f) 
            moonAngle = -40f;

        sunVerAngle = 10f*Mathf.Sin(t*180f/cycleDuration);
        if (t > 0f && t <= 0.5)
            moonVerAngle = -10f*Mathf.Sin((1-t)*180f/cycleDuration);
        else
            moonVerAngle = -10f*Mathf.Sin(t*180f/cycleDuration);

        moveSun(sunAngle, sunVerAngle);
        moveMoon(moonAngle, moonVerAngle);
    }

    void changeSkyColor(Color[] from, Color[] to)
    {
        float PhaseT = Mathf.Clamp01((t - offset)/divider);
        Color topColor = Color.Lerp(from[0], to[0], PhaseT);
        Color bottomColor = Color.Lerp(from[1], to[1], PhaseT);
        Color middleColor = Color.Lerp(from[2], to[2], PhaseT);

        runtimeSkybox.SetColor("_TopColor", topColor);
        runtimeSkybox.SetColor("_BottomColor", bottomColor);
        runtimeSkybox.SetColor("_MiddleColor", middleColor);
        runtimeSkybox.SetFloat("_BottomExponent", bottomExponent);
        runtimeSkybox.SetFloat("_TopExponent", topExponent);
    }

    void rotateClouds(float angle)
    {
        runtimeSkybox.SetFloat("_CloudsRotation", -angle);
    }

    void changeCloudsColor(Color from, Color to)
    {
        float PhaseT = Mathf.Clamp01((t - offset)/divider);
        Color cloudColor = Color.Lerp(from, to, PhaseT);
        runtimeSkybox.SetColor("_CloudsTint", cloudColor);
    }

    void moveSun(float angle, float verAngle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up)*
                       Quaternion.AngleAxis(verAngle, Vector3.right);
        Vector3 direction = q*Vector3.forward;    

        Matrix4x4 sunMatrix = Matrix4x4.TRS(Vector3.zero,
                                            q,
                                            Vector3.one);  

        runtimeSkybox.SetMatrix("sunMatrix", sunMatrix); 
    }

    void moveMoon(float angle, float verAngle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up)*
                       Quaternion.AngleAxis(verAngle, Vector3.right);

        Vector3 direction = q*Vector3.forward;        

        Matrix4x4 moonMatrix = Matrix4x4.TRS(Vector3.zero,
                                            q,
                                            Vector3.one);  

        runtimeSkybox.SetMatrix("moonMatrix", moonMatrix);
    }
}
