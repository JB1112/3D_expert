using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource, //자원
    Consumable // 소비 가능한 장비
}

public enum ConsumableType
{
    Hunger, //소비할 수 있는 아이템 종류 - 배고픔
    Health, // 체력 회복
    ChangeMoveSpeed //이동 속도 변경
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; //사용가능한 아이템 타입
    public float value; // 변경되는 값 value
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")] //파일 이름, 메뉴 이름을 지정함으로서 빠르게 생성가능
public class ItemData : ScriptableObject // ScriptableObject =  데이터를 사용하기 위한 스크립트
{
    [Header("Info")] //정보
    public string displayName; // 이름
    public string description; // 설명
    public ItemType type; // 각각 활용방법(타입)
    public Sprite icon; //아이콘 정보
    public GameObject dropPrefab; // 프리펩 저장

    [Header("Stacking")] // 가지고 있을 수 있는 개수
    public bool canStack; //여러 개 가질 수 있는지 Bool
    public int maxStackAmount; //최대 스택 개수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}