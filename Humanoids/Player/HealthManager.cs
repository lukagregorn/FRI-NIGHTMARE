using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public Humanoid player;

    private void Start() {
        UpdateHearts();
    }

    public void UpdateHearts() {
        
        int totalAmount = player.maxHealth / 2;
        int fullAmount = player.health / 2;

        // set active heart containers
        for (int i = 0; i < totalAmount; i++) {
            hearts[i].gameObject.SetActive(true);
            if (i < fullAmount)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }

        if (player.health % 2 != 0) {
            hearts[fullAmount].sprite = halfFullHeart;
        }
        
    }

}
