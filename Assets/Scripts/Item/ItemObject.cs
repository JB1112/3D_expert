using TMPro;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable // IInteractable 이라는 컴퍼넌트가 있다면 그 기능들을 사용할 수 있음
{
    public ItemData data; // 아이템 데이터를 넣어줌
    public GameObject Canvas;
    public TextMeshProUGUI Text;

    public void GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}"; //아이템의 이름과 설명을 띄우는 메세지
        Text.text = str;
        Canvas.SetActive(true);
        Invoke("ClosePrompt", 3f);
    }

    public void ClosePrompt()
    {
        Text.text = null;
        Canvas.SetActive(false);
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data; //플레이어에 직접 넘길 수 없기 때문에 CharacterManager를 통해 전달
        CharacterManager.Instance.Player.addItem?.Invoke(); // addItem에 필요한 기능을 구독시킴
        Destroy(gameObject); //키를 입력하면 아이템을 파괴
    }
}