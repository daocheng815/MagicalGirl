using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class character_image : Singleton<character_image>
{
    RectTransform rT;
    Image ui_image;

    public AnimationCurve curve;
    public Sprite[] image;
    public string[] imageName;

    readonly Dictionary<string, float> _imageW = new Dictionary<string, float>();
    readonly Dictionary<string, float> _imageH = new Dictionary<string, float>();

    public readonly Queue<IEnumerator> Queue = new Queue<IEnumerator>();

    public void Start()
    {
        ui_image = transform.GetChild(0).gameObject.GetComponent<Image>();
        rT = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        Disctionary_image_XandY();
        ImageActive(false);
    }
    //測試
    private void Test()
    {
        ui_image.sprite = image[0];
        var imagekeyW = _imageW.FirstOrDefault().Key;
        var imagekeyH = _imageH.FirstOrDefault().Key;
        rT.sizeDelta = new Vector2(_imageW[imagekeyW] * 20, _imageH[imagekeyH] * 20);
    }

    private void Disctionary_image_XandY()
    {
        for (int i = 0; i < image.Length; i++)
        {
            _imageW.Add(imageName[i], image[i].bounds.size.x);
            _imageH.Add(imageName[i], image[i].bounds.size.y);
        }
    }
    public void AddImage(string image_name, float Zoom_w = 1, float Zoom_h = 1, float offset_x = 0, float offset_y = 0)
    {
        ui_image.sprite = image[0];
        rT.sizeDelta = new Vector2(_imageW[image_name] * Zoom_w, _imageH[image_name] * Zoom_h);
        if (offset_x != 0 || offset_y != 0)
        {
            rT.anchoredPosition3D = new Vector2(offset_x, offset_y);
        }
    }
    public void ImageActive(bool Active)
    {
        transform.GetChild(0).gameObject.SetActive(Active);
    }
    public void Fade(float timedelay, bool def = true)
    {
        StartCoroutine(color_A(timedelay, def));
    }
    public IEnumerator color_A(float timedelay, bool def = true)
    {
        var timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            var myA = def ? Mathf.Lerp(0f, 1f, curve.Evaluate(timer / timedelay)) : Mathf.Lerp(1f, 0f, curve.Evaluate(timer / timedelay));
            ui_image.color = new(ui_image.color.r, ui_image.color.g, ui_image.color.b, myA);
            yield return null;
        }
    }
    public void Zoom(float timedelay, float xzoom = 1f, float yzoom = 1f)
    {
        StartCoroutine(_Zoom(timedelay, xzoom, yzoom));
    }

    private IEnumerator _Zoom(float timedelay,float xzoom, float yzoom)
    {
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float X_zoom = Mathf.Lerp(rT.localScale.x, xzoom, curve.Evaluate(timer / timedelay));
            float Y_zoom = Mathf.Lerp(rT.localScale.y, yzoom, curve.Evaluate(timer / timedelay));
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
        var anchoredPosition = rT.anchoredPosition;
        var x_offset = anchoredPosition.x;
        var y_offset = anchoredPosition.y;
        var timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float my_x = Mathf.Lerp(x_offset, x_offset + xoffset, curve.Evaluate(timer / timedelay));
            float my_y = Mathf.Lerp(y_offset, y_offset + yoffset, curve.Evaluate(timer / timedelay));
            rT.anchoredPosition3D = new Vector2(my_x, my_y);
            yield return null;
        }
    }
    public void Pause(float timedelay)
    {
        StartCoroutine(ThisPause(timedelay));
        IEnumerator ThisPause(float timedelay)
        {
            yield return new WaitForSeconds(timedelay);
        }
    }

    public void zoom_num(int num)
    {
        StartCoroutine(ZoomN(num));
        IEnumerator ZoomN(int num)
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
        while (Queue.Count > 0)
        {
            IEnumerator operation = Queue.Dequeue();
            yield return StartCoroutine(operation);
        }
    }
}
