using DG.Tweening;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    private int characterCount = 2;
    
    public Image[] highlightedCharacterImages;
    public Color highlightedColor, normalColor;
    
    public TMP_Text[] selectedCharacterTexts;
    
    public TMP_Text playerID;

    public GameObject bitmanSelectedText;
    
    public int playerIndex;
    private int characterIndex;
    
    [SerializeField] private EventReference BITMANSelectSound;

    private bool canChange;
    private bool selected;

    private int ignoreFirstInput;

    public void Init(int index)
    {
        bitmanSelectedText.SetActive(false);
        canChange = true;
        transform.SetParent(CharacterSelectUI.instance.transform);
        playerIndex = index;
        UpdateUI();
        playerID.text = "Player " + (playerIndex + 1).ToString();
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        
        characterCount = highlightedCharacterImages.Length - 1;
    }

    public void OnNavigate(InputValue value)
    {
        ignoreFirstInput++;
        
        if (selected || ignoreFirstInput < 2)
            return;
        
        float y = value.Get<Vector2>().y;
        float x = value.Get<Vector2>().x;

        if ((y < -.5 || y > .5) && canChange)
        {
            canChange = false;

            if (Mathf.Sign(y) == 1)       // stick pushed UP
            {
                characterIndex -= 2;
                
                if (characterIndex < 0)
                {
                    characterIndex += 2;
                }
            }
            else                            // stick pushed DOWN
            {
                characterIndex += 2;
                
                if (characterIndex > characterCount)
                {
                    characterIndex -= 2;
                }
            }
            UpdateUI();
        }
        else if (y == 0)
        {
            canChange = true;
        }
        
        if ((x < -.5 || x > .5) && canChange)
        {
            canChange = false;

            if (Mathf.Sign(x) == 1)       // stick pushed RIGHT
            {
                characterIndex++;         
                
                if (characterIndex > characterCount)
                {
                    characterIndex--;
                }
            }
            else                            // stick pushed LEFT
            {
                characterIndex--;   
                
                if (characterIndex < 0)
                {
                    characterIndex++;
                }
            }
            UpdateUI();
        }
        else if (x == 0)
        {
            canChange = true;
        }
    }

    private readonly int bitmanRequiredPresses = 15;
    private int currentPresses = 0;
    private bool selectionLocked = false;

    public void OnBITMANCHEAT(InputValue value)
    {
        if (value.isPressed)
        {
            currentPresses++;
            // Debug.Log(currentPresses);
            
            if (currentPresses >= bitmanRequiredPresses && !selected && !selectionLocked)
            {
                CharacterSelectionManager.Instance.OnCharacterDeselected(playerIndex);
                selected = false;
                // UpdateUI();
                Debug.Log("CHOSEN BITMAN NO WAY BACK");
                bitmanSelectedText.SetActive(true);
                characterIndex = 4;
                selectionLocked = true;
                selected = true;
                CharacterSelectionManager.Instance.OnCharacterSelect(playerIndex, characterIndex);
                
                RuntimeManager.PlayOneShot(BITMANSelectSound, this.transform.position);
            }
        }
    }

    public void OnSelect(InputValue value)
    {
        ignoreFirstInput++;

        if (ignoreFirstInput < 2)
            return;
        
        if (value.isPressed)
        {
            // Debug.Log($"OnSelect fired for playerIndex={playerIndex}, characterIndex={characterIndex}");
            
            CharacterSelectionManager.Instance.OnCharacterSelect(playerIndex, characterIndex);
            selected = true;
            UpdateUI();
        }
    }

    public void OnDeselect(InputValue value)
    {
        if (selectionLocked)
            return;
        
        if (!value.isPressed) 
            return;
    
        if (!selected) 
            return; 
    
        CharacterSelectionManager.Instance.OnCharacterDeselected(playerIndex);
        selected = false;
        UpdateUI();
    }



    private void UpdateUI()
    {
        if (selected)
        {
            AudioManager.instance.PlayButtonOnClicked();
            
            selectedCharacterTexts[characterIndex].enabled = true;
            highlightedCharacterImages[characterIndex].rectTransform
                .DOShakeAnchorPos(0.2f, 50, 100, 90);
            
            // play sound based on character
            switch (characterIndex)
            {
                case 0:
                    AudioManager.instance.PlayCharSelectSword();
                    break;
                case 1:
                    AudioManager.instance.PlayCharSelectAxeman();
                    break;
                case 2:
                    AudioManager.instance.PlayCharSelectReaper();
                    break;
                case 3:
                    AudioManager.instance.PlayCharSelectTwinblade();
                    break;
                        
            }
        }
        else
        {
            selectedCharacterTexts[characterIndex].enabled = false;
            
            // Play sound on hover 
            AudioManager.instance.PlayButtonOnClickedSlider();
            
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