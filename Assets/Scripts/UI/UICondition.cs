using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina; // 각자의 컨디션 UI

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this; //Player에 있는 uiCondition에 자신을 넣어줌
    }
}