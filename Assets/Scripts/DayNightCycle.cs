using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)] //범위 0~1의 값 (0% ~ 100%)
    public float time;
    public float fullDayLength; // 하루 길이
    public float startTime = 0.4f; //0.5일때 정오시각
    private float timeRate;
    public Vector3 noon; //정오 = Vector 90 0 0

    [Header("Sun")] //태양 관련 변수
    public Light sun;
    public Gradient sunColor; // Gradient - 서서히 변화
    public AnimationCurve sunIntensity; //서서히 증가/감소

    [Header("Moon")]//달 관련 변수
    public Light moon;
    public Gradient moonColor;// Gradient - 서서히 변화
    public AnimationCurve moonIntensity; //서서히 증가/감소

    [Header("Other Lighting")] //기타 빛
    public AnimationCurve lightingIntensityMultiplier; // Lighting 탭에 있던 옵션
    public AnimationCurve reflectionIntensityMultiplier; // Lighting 탭에 있던 옵션

    private void Start()
    {
        timeRate = 1.0f / fullDayLength; // 시간변화
        time = startTime; //24시간중 40% 진행중 - 9시 이후?
    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f; // 시간= 시간 변화율*deltaTime 

        UpdateLighting(sun, sunColor, sunIntensity); //태양 업데이트
        UpdateLighting(moon, moonColor, moonIntensity); //달 업데이트

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve) //빛을 업데이트 해주는 메서드
    {
        float intensity = intensityCurve.Evaluate(time); //보간되는 값을 받아옴

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        //각도를 변환시키는 공식. 0.5일때 정오여야 하지만 실제 태양은 360도이기 때문에 0.25일때 정오임 그래서 0.25를 빼줌 + 4.0을 곱해줌으로서 올바른 각도를 구함
        lightSource.color = colorGradiant.Evaluate(time); //컬러도 시간에 따라 보간됨
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject; // 게임오브젝트를 받아옴
        if (lightSource.intensity == 0 && go.activeInHierarchy) //intensity기 0일경우 창에서 내림
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy) //0보다 커질 경우 활성화
            go.SetActive(true);
    }
}
