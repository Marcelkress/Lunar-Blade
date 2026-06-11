using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    private int _highlighted = 0;
    [SerializeField] private int characterCount = 2;
    
    public Image[] highlightedCharacterImages;
    
    public int playerIndex;
    private int characterIndex;

    private bool canChange;

    public void Init(int index)
    {
        canChange = true;
        transform.SetParent(CharacterSelectUI.instance.transform);
        playerIndex = index;
        UpdateUI();
    }

    // Matches Send Messages style
    public void OnNavigate(InputValue value)
    {
        float y = value.Get<Vector2>().y;

        if ((y < -.5 || y > .5) && canChange)
        {
            canChange = false;
            characterIndex += (int)y;

            if (characterIndex >= characterCount)
            {
                characterIndex = 0;
            }
            else if (characterIndex < 0)
            {
                characterIndex = characterCount - 1;
            }
        }
        else if (y == 0)
        {
            canChange = true;
        }
        
        UpdateUI();
    }

    public void OnSelect(InputValue value)
    {
        Debug.Log("OnSelect");
        if (value.isPressed)
        {
            CharacterSelectionManager.Instance.OnCharacterSelect(playerIndex, characterIndex);
            highlightedCharacterImages[characterIndex].DOColor(Color.white, 0.3f);
            highlightedCharacterImages[characterIndex].rectTransform
                .DOShakeAnchorPos(0.2f, 100, 10, 90);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < highlightedCharacterImages.Length; i++)
        {
            if (i == characterIndex)
            {
                highlightedCharacterImages[i].DOFade(1, 0.2f);
            }
            else
            {
                highlightedCharacterImages[i].DOFade(.5f, 0.2f);
            }
        }
    }
}