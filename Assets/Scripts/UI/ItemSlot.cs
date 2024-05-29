using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item; //슬롯에 들어갈 아이템 데이터

    public UIInventory inventory; //UI 인벤토리 정보
    public Button button; //버튼
    public Image icon; //아이콘
    public TextMeshProUGUI quatityText; //숫자 텍스트
    private Outline outline; //클릭시 바뀌는 OutLine

    public int index; //아이템 슬롯 정보(순서)
    public bool equipped; // 장착된 장비인지 아닌지 판단
    public int quantity; // 숫자(양) 정보

    private void Awake()
    {
        outline = GetComponent<Outline>(); //OutLine은 GetComponent로 세팅
    }

    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    public void Set() //아이템을 슬롯에 세팅
    {
        icon.gameObject.SetActive(true); //아이콘 활성화
        icon.sprite = item.icon; //스프라이트에 아이콘 넣어줌
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty; //양이 1보다 크다면 숫자 입력, 아니라면 숫자 표기 X

        if (outline != null) //방어코드 OutLine이 활성화 되지 않을경우를 대비
        {
            outline.enabled = equipped;
        }
    }

    public void Clear() //아이템 슬롯에서 지우기
    {
        item = null; //아이템 비우기
        icon.gameObject.SetActive(false); //아이콘 비활성화
        quatityText.text = string.Empty; //스트링 비우기
    }

    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
