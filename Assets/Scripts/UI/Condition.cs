using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue; //현재 값
    public float maxValue; // 최대 값
    public float startValue; // 시작시 적용할 값
    public float regenRate; // 회복 비율
    public float decayRate; // 감소 비율
    public Image uiBar; //UI 이미지

    private void Start()
    {
        curValue = startValue; //현재 값을 시작시 적용할 값으로 변경
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage(); // UI바의 fillAmount 값을 현재값/최대값으로 적용
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue); //현재 값에 더해주기 (Mathf.Min - 둘 중 최소값을 가져오기)
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f); // 현재 값에 빼주기 (Mathf.Max - 둘 중 최대값을 가져오기)
    }

    public float GetPercentage()
    {
        return curValue / maxValue; // 현재값 계산 메서드
    }
}