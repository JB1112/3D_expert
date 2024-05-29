using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour //외부에서 Palyer에 접근하고 싶어할 때 Player 스크립트를 통해 접근 가능
{
    public PlayerController controller; //플레이어 컨트롤러 호출
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this; // 캐릭터 매니저에 존재하는 Player에 자신을 넣어줌
        controller = GetComponent<PlayerController>(); //GetComponent로 PlayerController 넣어줌
        condition = GetComponent<PlayerCondition>();
    }
}