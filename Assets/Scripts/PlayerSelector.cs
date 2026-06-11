using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    private int _highlighted = 0;
    [SerializeField] private int characterCount = 2;
    public Button[] characterButtons;
    public int playerIndex;
    private int characterIndex;

    public void Init(int index)
    {
        transform.SetParent(CharacterSelectUI.instance.transform);
        playerIndex = index;
        UpdateUI();
    }

    // Matches Send Messages style
    public void OnNavigate(InputValue value)
    {
        float y = value.Get<Vector2>().y;

        if (y > 0.5f)       _highlighted = (_highlighted + 1) % characterCount;
        else if (y < -0.5f) _highlighted = (_highlighted - 1 + characterCount) % characterCount;

        UpdateUI();
    }

    public void OnSelect(InputValue value)
    {
        Debug.Log("OnSelect");
        if (value.isPressed)
        {
            CharacterSelectionManager.Instance.OnCharacterSelect(playerIndex, characterIndex);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < characterButtons.Length; i++)
        {
            characterButtons[i].interactable = (i == _highlighted);
        }
        
        characterIndex = (characterIndex + 1) % characterCount;
    }
}