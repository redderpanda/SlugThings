using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWall : MonoBehaviour {
    public GameObject Switch;
    public GameObject Wall;
    public GameObject Blockade;

    // Use this for initialization
    void Start()
    {
        Wall.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool IsColorSomewhatEqual(Color player, Color goal, float leniency)
    {

        if ((goal.r - leniency) > player.r || player.r > (goal.r + leniency))
            return false;

        if ((goal.g - leniency) > player.g || player.g > (goal.g + leniency))
            return false;

        if ((goal.b - leniency) > player.b || player.b > (goal.b + leniency))
            return false;

        return true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Color p_Color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            Color s_Color = Switch.GetComponent<SpriteRenderer>().color;
            if (CheckRatio(p_Color) == CheckRatio(s_Color) || IsColorSomewhatEqual(p_Color, s_Color, 0.1f)){
                Wall.SetActive(false);
                Blockade.SetActive(true);
            }else{
                Wall.SetActive(true);
                Blockade.SetActive(false);
            }

        }
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


}
