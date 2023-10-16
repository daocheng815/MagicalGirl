using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class character_image : Singleton<character_image>
{
    RectTransform rT;
    Image ui_image;

    public AnimationCurve Curve;
    public Sprite[] image;
    public string[] image_name;

    Dictionary<string, float> image_W = new Dictionary<string, float>();
    Dictionary<string, float> image_H = new Dictionary<string, float>();

    public Queue<IEnumerator> queue = new Queue<IEnumerator>();

    void Start()
    {
        ui_image = transform.GetChild(0).gameObject.GetComponent<Image>();
        rT = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        Disctionary_image_XandY();
        ImageActive(false);
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
    public void AddImage(string image_name, float Zoom_w = 1, float Zoom_h = 1, float offset_x = 0, float offset_y = 0)
    {
        ui_image.sprite = image[0];
        rT.sizeDelta = new Vector2(image_W[image_name] * Zoom_w, image_H[image_name] * Zoom_h);
        if (offset_x != 0 || offset_y != 0)
        {
            rT.anchoredPosition3D = new Vector2(offset_x, offset_y);
        }
    }
    public void ImageActive(bool Active)
    {
        transform.GetChild(0).gameObject.SetActive(Active);
    }
    public void fade(float timedelay, bool def = true)
    {
        StartCoroutine(color_A(timedelay, def));
    }
    public IEnumerator color_A(float timedelay, bool def = true)
    {
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float my_a;
            if (def)
                my_a = Mathf.Lerp(0f, 1f, Curve.Evaluate(timer / timedelay));
            else
                my_a = Mathf.Lerp(1f, 0f, Curve.Evaluate(timer / timedelay));
            ui_image.color = new(ui_image.color.r, ui_image.color.g, ui_image.color.b, my_a);
            yield return null;
        }
    }
    public void Zoom(float timedelay, float Xzoom = 1f, float Yzoom = 1f)
    {
        StartCoroutine(_Zoom(timedelay, Xzoom, Yzoom));
    }
    public IEnumerator _Zoom(float timedelay,float Xzoom, float Yzoom)
    {
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float X_zoom = Mathf.Lerp(rT.localScale.x, Xzoom, Curve.Evaluate(timer / timedelay));
            float Y_zoom = Mathf.Lerp(rT.localScale.y, Yzoom, Curve.Evaluate(timer / timedelay));
            rT.localScale = new Vector2(X_zoom, Y_zoom);
            yield return null;
        }
    }
    public void Offset(float timedelay, float xoffset = 0f, float yoffset = 0f)
    {
        StartCoroutine(_Offset(timedelay, xoffset, yoffset));
    }
    public IEnumerator _Offset(float timedelay, float xoffset = 0f, float yoffset = 0f)
    {
        float x_offset = rT.anchoredPosition.x;
        float y_offset = rT.anchoredPosition.y;
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float my_x = Mathf.Lerp(x_offset, x_offset + xoffset, Curve.Evaluate(timer / timedelay));
            float my_y = Mathf.Lerp(y_offset, y_offset + yoffset, Curve.Evaluate(timer / timedelay));
            rT.anchoredPosition3D = new Vector2(my_x, my_y);
            yield return null;
        }
    }
    public void Pause(float timedelay)
    {
        StartCoroutine(pause(timedelay));
        IEnumerator pause(float timedelay)
        {
            yield return new WaitForSeconds(timedelay);
        }
    }

    public void zoom_num(int num)
    {
        StartCoroutine(zoom_n(num));
        IEnumerator zoom_n(int num)
        {
            for (int i = 0; i < num; i++)
            {
                yield return StartCoroutine(_Zoom(0.05f, 1.2f, 1.2f));
                yield return StartCoroutine(_Zoom(0.05f, 1f, 1f));
            }
        }
    }
    public IEnumerator QueueExecute()
    {
        while (queue.Count > 0)
        {
            IEnumerator operation = queue.Dequeue();
            yield return StartCoroutine(operation);
        }
    }
}
