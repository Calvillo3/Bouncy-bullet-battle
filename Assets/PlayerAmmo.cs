using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAmmo : MonoBehaviour
{
    // Start is called before the first frame update
   
    public TextMeshProUGUI ammoText;
    public Image gunImage;
    [SerializeField] Sprite akSprite;
    [SerializeField] Sprite shotgunSprite;
    [SerializeField] Sprite pistolSprite;

    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        int playerNum = playerMovement.playerNum;
        if (playerNum == 1)
        {
            ammoText = GameObject.Find("GreenAmmo").GetComponent<TextMeshProUGUI>();
            gunImage = GameObject.Find("GreenGun").GetComponent<Image>();
        }
        else
        {
            ammoText = GameObject.Find("BlueAmmo").GetComponent<TextMeshProUGUI>();
            gunImage = GameObject.Find("BlueGun").GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.ammoCount > 0)
        {
            ammoText.text = playerMovement.ammoCount.ToString();
            if (playerMovement.bulletType[1])
            {
                gunImage.sprite = akSprite;
            }
            else if (playerMovement.bulletType[2])
            {
                gunImage.sprite = shotgunSprite;
            }
        }
        else
        {
            gunImage.sprite = pistolSprite;
            ammoText.text = "\u221E";
        }
    }
}
