using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SpecialAttackDisplay : MonoBehaviour
{
    [Header("Character display sprites")] 
    public Sprite[] characterSprites;
    
    public static SpecialAttackDisplay instance;
    private Image image;
    private Animator anim;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.DOFade(0, 0);
    }
    
    public void DisplayCharacter(int characterIndex)
    {
        image.sprite = characterSprites[characterIndex];
        anim.Play("SpecialUIAnim", 0, 0f);
    }
    
}
