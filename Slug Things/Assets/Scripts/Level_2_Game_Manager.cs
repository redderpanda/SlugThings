using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_2_Game_Manager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Color exit_color;
    public bool player1set;

    // Use this for initialization
    void Start()
    {
        //exit_color = new Color(1, 0, 1);
        player1set = false;
    }
    public Color CheckRatio(Color player)
    {
        float red, green, blue;
        float max = 0f;
        float min = 1000f;
        Color result;
        red = player.r;
        green = player.g;
        blue = player.b;
        if (red > max)
            max = red;
        if (red < min && red > 0f)
            min = red;
        if (green > max)
            max = green;
        if (green < min && green > 0f)
            min = green;
        if (blue > max)
            max = blue;
        if (blue < min && blue > 0f)
            min = blue;
        if (min != max)
        {
            result = new Color(Mathf.Clamp(red / max, 0f, 1f), Mathf.Clamp(green / max, 0f, 1f), Mathf.Clamp(blue / max, 0f, 1f));
        }
        else
        {
            result = new Color(red, green, blue);
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        //if (player1.GetComponent<SpriteRenderer>().color == exit_color || player2.GetComponent<SpriteRenderer>().color == exit_color)
        //Debug.Log(player1.GetComponent<SpriteRenderer>().color.r);
        //Debug.Log(player1.GetComponent<SpriteRenderer>().color.g);
        //Debug.Log(player1.GetComponent<SpriteRenderer>().color.b);
//        if (CheckRatio(player1.GetComponent<SpriteRenderer>().color) == yellow_block.GetComponent<SpriteRenderer>().color || CheckRatio(player2.GetComponent<SpriteRenderer>().color) == orange_block.GetComponent<SpriteRenderer>().color)
//        {
//            //Color temp = exit.GetComponent<SpriteRenderer>().color;
//            //temp.a = 1f;
//            //exit.GetComponent<SpriteRenderer>().color = temp;
//            //exit.GetComponent<BoxCollider2D>().enabled = true;
//            yellow_trigger.SetActive(true);
//        }
//        if (CheckRatio(player1.GetComponent<SpriteRenderer>().color) == orange_block.GetComponent<SpriteRenderer>().color || CheckRatio(player2.GetComponent<SpriteRenderer>().color) == orange_block.GetComponent<SpriteRenderer>().color)
//        {
//            orange_trigger.SetActive(true);
//        }
        //else
        //{
        //    Color temp = exit.GetComponent<SpriteRenderer>().color;
        //    temp.a = 0.2f;
        //    exit.GetComponent<SpriteRenderer>().color = temp;
        //    exit.GetComponent<BoxCollider2D>().enabled = false;
        //}
    }
}
