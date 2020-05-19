using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.OnDeath += ShowPanel;

        gameObject.SetActive(false);
    }

    private void ShowPanel()
    {
        gameObject.SetActive(true);
    }

}
