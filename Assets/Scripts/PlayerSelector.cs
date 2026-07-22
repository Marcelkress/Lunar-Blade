using DG.Tweening;
using TMPro;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    private int _highlighted = 0;
    [SerializeField] private int characterCount = 2;
    
    public Image[] highlightedCharacterImages;
    public Color highlightedColor, normalColor;
    
    public TMP_Text[] selectedCharacterTexts;
    
    public TMP_Text playerID;
    
    public int playerIndex;
    private int characterIndex;

    private bool canChange;
    private bool selected;

    public void Init(int index)
    {
        canChange = true;
        transform.SetParent(CharacterSelectUI.instance.transform);
        playerIndex = index;
        UpdateUI();
        playerID.text = "Player " + (playerIndex + 1).ToString();
    }

    public void OnNavigate(InputValue value)
    {
        if (selected)
            return;
        
        float y = value.Get<Vector2>().y;

        if ((y < -.5 || y > .5) && canChange)
        {
            canChange = false;

            if (Mathf.Sign(y) == 1)       // stick pushed UP
            {
                characterIndex--;          // move to previous (upward) image
            }
            else                            // stick pushed DOWN
            {
                characterIndex++;          // move to next (downward) image
            }
            
            if (characterIndex >= characterCount)
            {
                characterIndex = 0;
            }
            else if (characterIndex < 0)
            {
                characterIndex = characterCount - 1; // fix off-by-one here too
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
        if (value.isPressed)
        {
            CharacterSelectionManager.Instance.OnCharacterSelect(playerIndex, characterIndex);
            selected = true;
            UpdateUI();
        }
    }

    public void OnDeselect(InputValue value)
    {
        CharacterSelectionManager.Instance.OnCharacterDeselected(playerIndex);
        selected = false;
        UpdateUI();
    }



    private void UpdateUI()
    {
        if (selected)
        {
            selectedCharacterTexts[characterIndex].enabled = true;
            highlightedCharacterImages[characterIndex].rectTransform
                .DOShakeAnchorPos(0.2f, 50, 100, 90);
        }
        else
        {
            selectedCharacterTexts[characterIndex].enabled = false;
            
            for (int i = 0; i < highlightedCharacterImages.Length; i++)
            {
                if (i == characterIndex)
                {
                    highlightedCharacterImages[i].DOColor(highlightedColor, 0.3f);
                }
                else
                {
                    highlightedCharacterImages[i].DOColor(normalColor, 0.3f);
                }
            }
        }
    }
}