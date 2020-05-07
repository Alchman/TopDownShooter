using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI2 : MonoBehaviour
{
    public Zombie zombie;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = 1;
        zombie.onHealthChanged += UpdateSlider;
    }
    void UpdateSlider()
    {
        healthBar.fillAmount = (float)zombie.health / (float)zombie.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
