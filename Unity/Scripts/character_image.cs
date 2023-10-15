using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class character_image : Singleton<character_image>
{
    RectTransform rT;
    Image ui_image;

    public AnimationCurve Curve;
    public Sprite[] image;
    public string[] image_name;

    Dictionary<string,float> image_W = new Dictionary<string, float>();
    Dictionary<string, float> image_H = new Dictionary<string, float>();
    
    void Start()
    {
        ui_image = transform.GetChild(0).gameObject.GetComponent<Image>();
        rT = transform.GetChild(0).gameObject.GetComponent< RectTransform>();

        Disctionary_image_XandY();
        ImageActive(false);

        ImageActive(true);
        AddImage("player_idle", 20, 20, 190, -220);
    }

    void test()
    {
        ui_image.sprite = image[0];
        var imagekey_W = image_W.FirstOrDefault().Key;
        var imagekey_H = image_H.FirstOrDefault().Key;
        rT.sizeDelta = new Vector2(image_W[imagekey_W] * 20, image_H[imagekey_H] * 20);
    }

    void Disctionary_image_XandY()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image_W.Add(image_name[i], image[i].bounds.size.x);
            image_H.Add(image_name[i], image[i].bounds.size.y);
        }
    }

    public void AddImage(string image_name,float Zoom_w = 1, float Zoom_h = 1,float offset_x = 0, float offset_y = 0)
    {
        ui_image.sprite = image[0];
        rT.sizeDelta = new Vector2(image_W[image_name] * Zoom_w, image_H[image_name] * Zoom_h);
        if (offset_x != 0 || offset_y != 0)
        {
            rT.anchoredPosition3D = new Vector2(offset_x, offset_y);
        }
        StartCoroutine(color_A(2f));
        IEnumerator color_A(float timedelay)
        {
            float timer = 0f;
            while (timer < timedelay)
            {
                float my_a = Mathf.Lerp(1f, 0f, Curve.Evaluate(timer / timedelay));
                ui_image.color = new(ui_image.color.r, ui_image.color.g, ui_image.color.b, my_a);
                yield return null;
            }
        }
    }
    
    public void Offset_Ani(float timedelay , float x = 0f , float y = 0f)
    {
        float x_o = rT.anchoredPosition.x;
        float y_o = rT.anchoredPosition.y;
        StartCoroutine(Offset(timedelay));
        IEnumerator Offset(float timedelay)
        {
            float timer = 0f;
            
            while (timer < timedelay)
            {
                float my_x = Mathf.Lerp(x_o, x, Curve.Evaluate(timer / timedelay));
                float my_y = Mathf.Lerp(y_o, y, Curve.Evaluate(timer / timedelay));
                rT.anchoredPosition3D = new Vector2(my_x, my_y);
                yield return null;
            }
        }
    }
    public void ImageActive(bool Active)
    {
        transform.GetChild(0).gameObject.SetActive(Active);
    }
}
