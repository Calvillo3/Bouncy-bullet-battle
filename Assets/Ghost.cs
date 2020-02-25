using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

    float timeEntered;
    [SerializeField] float reviveTime;

    [SerializeField] float blinkPeriod;
    float nextBlink;
    SpriteRenderer ren;
    [SerializeField] float colorFade;
    [SerializeField] Image progress;


    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<SpriteRenderer>();
        ren.color = player.GetComponent<SpriteRenderer>().color;
        progress.color = ren.color;
        ren.color = Color.Lerp(ren.color, Color.white, colorFade);


        timeEntered = Mathf.Infinity;
        nextBlink = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progress.fillAmount = Mathf.Clamp((Time.time - timeEntered) / reviveTime, 0, 1);
        if (Time.time > timeEntered + reviveTime)
        {
            Blink();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (layerName == "Player")
        {
            timeEntered = Time.time;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (layerName == "Player")
        {
            if (Time.time > (timeEntered + reviveTime))
            {
                Vector2 playerPos = player.rb.position;
                player.rb.position = this.gameObject.transform.position;
                this.gameObject.transform.position = playerPos;

                ren.enabled = true;

                player.Revive();
                timeEntered = Mathf.Infinity;
                this.gameObject.SetActive(false);
            }
            else
            {
                timeEntered = Mathf.Infinity;
            }
        }
    }

    void Blink()
    {
        if (Time.time > nextBlink)
        {
            ren.enabled = !ren.enabled;
            nextBlink = Time.time + blinkPeriod;
        }
    }
}
