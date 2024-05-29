using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots; //각 슬롯의 정보롤 배열로 저장

    public GameObject inventoryWindow; // 인벤토리 창
    public Transform slotPanel; // 슬롯 판넬
    public Transform dropPosition; //아이템을 드롭할 위치

    [Header("Selected Item")] //선택 아이템 관련
    private ItemSlot selectedItem; //선택한 아이템
    private int selectedItemIndex; //선택한 아이템의 인덱스
    public TextMeshProUGUI selectedItemName; // 선택된 아이템의 이름 및 정보
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton; // 각 버튼 입력
    public GameObject dropButton;
    private float currentSpeed;

    private int curEquipIndex; // 장착된 아이템의 인덱스 정보

    private PlayerController controller; //아이템 사용을 위한 플레이어 컨트롤러
    private PlayerCondition condition; // 플레이어 상태

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller; //컨트롤러 입력
        condition = CharacterManager.Instance.Player.condition; // 컨디션 입력
        dropPosition = CharacterManager.Instance.Player.dropPosition; //캐릭터 인스턴스에 있는 드롭포지션을 입력
        
        currentSpeed = controller.moveSpeed;

        controller.inventory += Toggle; //컨트롤러에 있는 Inventory에 Toggle 입력
        CharacterManager.Instance.Player.addItem += AddItem; //Add Item 구독시킴

        inventoryWindow.SetActive(false); //인벤토리는 시작할 때 비활성화 상태
        slots = new ItemSlot[slotPanel.childCount]; // 슬롯 판넬을 가져와서 그 자식(Child = slot)의 숫자만큼 배열 생성

        for (int i = 0; i < slots.Length; i++) // 슬롯 판넬의 개수만큼 정보 등록
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i; // 슬롯의 인덱스 등록
            slots[i].inventory = this; //인벤토리에 넣기
            slots[i].Clear(); 
        }

        ClearSelectedItemWindow(); // 시작 인벤토리 초기화
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy) //인벤토리가 상태 체크 후 토글 동작
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen() //인벤토리창이 켜져있는지 체크함
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData; //아이템 데이터를 받아와서 추가

        if (data.canStack) //아이템이 중복 가능할 때 사용됨
        {
            ItemSlot slot = GetItemStack(data); //슬롯에 GetItemStack 
            if (slot != null) //슬롯이 null이 아니라면 (이미 가지고 있다면)
            {
                slot.quantity++; //슬롯의 양을 증가
                UpdateUI(); //UI 업데이트
                CharacterManager.Instance.Player.itemData = null; //Player의 아이템 데이터를 초기화
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot(); //중복이 불가능하다면 비어있는 슬롯 가져옴

        if (emptySlot != null) //비어있는 슬롯이 있다면
        {
            emptySlot.item = data; //아이템데이터 입력
            emptySlot.quantity = 1; //숫자 1로 증가
            UpdateUI(); //UI 업데이트
            CharacterManager.Instance.Player.itemData = null; //Player의 아이템 데이터 초기화
            return;
        }

        ThrowItem(data); //슬롯이 없는 경우 버리기
        CharacterManager.Instance.Player.itemData = null; //Player의 아이템 데이터 초기화
    }

    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360)); //생성 오브젝트와 위치, 드롭 각도 조정
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++) //슬롯을 순회하며
        {
            if (slots[i].item != null) //데이터가 들어가 있다면
            {
                slots[i].Set();  //세팅을 하고
            }
            else
            {
                slots[i].Clear(); //지우기
            }
        }
    }

    ItemSlot GetItemStack(ItemData data) // 중첩 가능한 아이템 스택 늘리기
    {
        for (int i = 0; i < slots.Length; i++) //슬롯을 순회하며
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount) //데이터가 넣으려는 데이터와 같고, 최대값보다 작으면
            {
                return slots[i]; //슬롯을 반환함
            }
        }
        return null; // 아니라면 null값 리턴
    }

    ItemSlot GetEmptySlot() // 빈 슬롯 가져오기
    {
        for (int i = 0; i < slots.Length; i++) //슬롯을 순회하며
        {
            if (slots[i].item == null) //아이템이 null인 슬롯을 반환
            {
                return slots[i];
            }
        }
        return null;
    }

    public void SelectItem(int index) //선택한 아이템의 인덱스
    {
        if (slots[index].item == null) return; //인벤토리 정보가 없다면 돌아감

        selectedItem = slots[index]; //index번호의 아이템 삽입
        selectedItemIndex = index; // index 번호 삽입

        selectedItemName.text = selectedItem.item.displayName; //선택한 아이템의 이름
        selectedItemDescription.text = selectedItem.item.description; // 선택한 아이템의 설명

        selectedItemStatName.text = string.Empty; //아이템의 스탯 유무에 차이 때문에 임시적으로 비움
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++) // Consumables가 존재한다면
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n"; // i번째의 타입을 입력
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n"; // i번째 아이템의 값을 입력
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable); //Consumable이라면 사용하기 버튼 가져오기
        dropButton.SetActive(true); //떨구기 버튼 활성화
    }

    void ClearSelectedItemWindow() // 초기 창 클리어
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable) //아이템이 사용 가능하다면
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type) //Consumable의 타입 체크
                {
                    case ConsumableType.Health: //health 라면
                        condition.Heal(selectedItem.item.consumables[i].value); break; //heal사용
                    case ConsumableType.Hunger: // Hunger라면
                        condition.Eat(selectedItem.item.consumables[i].value); break; //Eat 사용
                    case ConsumableType.ChangeMoveSpeed:
                        controller.moveSpeed = controller.moveSpeed * selectedItem.item.consumables[i].value;
                        Invoke("RestoreSpeed", 3.0f);
                        break;
                }
            }
            RemoveSelctedItem(); //선택한 아이템 지우기
        }
    }

    private void RestoreSpeed()
    {
        controller.moveSpeed = currentSpeed;
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item); // 선택한 아이템 버리기
        RemoveSelctedItem(); // 선택한 아이템 인벤토리에서 제거
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--; //선택한 아이템의 개수를 줄임

        if (selectedItem.quantity <= 0) //아이템의 양이 0보다 작거나 같을 경우
        {
            if (slots[selectedItemIndex].equipped)
            {
                //UnEquip(selectedItemIndex);
            }

            selectedItem.item = null; // 선택 아이템 지우고
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow(); // 선택한 아이템정보 날리기
        }

        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}