using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public Sprite damagedSprite;
    public int hp = 4;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void damageWall(int loss)
    {
        spriteRenderer.sprite = damagedSprite;
        hp -= loss;

        if(hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
