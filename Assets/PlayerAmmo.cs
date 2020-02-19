using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAmmo : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField] GameObject player;
    public TextMeshProUGUI scoreText;
    public Image gunImage;
    [SerializeField] Sprite akSprite;
    [SerializeField] Sprite shotgunSprite;
    [SerializeField] Sprite pistolSprite;

    PlayerMovement playerMovement;
    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerMovement>().ammoCount > 0)
        {
            scoreText.text = playerMovement.ammoCount.ToString();
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
            scoreText.text = "\u221E";
        }
    }
}
