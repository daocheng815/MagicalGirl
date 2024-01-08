using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class character_image : Singleton<character_image>
{
    RectTransform rT;
    //紀錄UI物件列表
    [SerializeField]private List<RectTransform> rTList = new List<RectTransform>();
    //圖片物件列表
    [SerializeField]private List<Image> imageList = new List<Image>();

    public GameObject imageObjPrefab;
    
    public AnimationCurve curve;
    public Sprite[] image;
    public string[] imageName;

    Dictionary<string, float> _imageW = new Dictionary<string, float>();
    Dictionary<string, float> _imageH = new Dictionary<string, float>();

    public readonly Queue<IEnumerator> Queue = new Queue<IEnumerator>();

    public void Start()
    {
        AddImageObj(5);
        //ui_image = transform.GetChild(0).gameObject.GetComponent<Image>();
        rT = transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        ObjectSearch();
        Disctionary_image_XandY();
        ImageActive(false,0);
        Test();
    }
    
    private void Test()
    {
        
        //DebugTask.Log(_imageW["black"] + "  " +_imageH["black"] + "        "+_imageW["player_idle"] + "  " +_imageH["player_idle"]);
    }
    
    private void ObjectSearch()
    {
        Transform parentTransform = transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            Image imageComponent = child.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageList.Add(imageComponent);
            }
            RectTransform RTComponent = child.GetComponent<RectTransform>();
            if (RTComponent != null)
            {
                rTList.Add(RTComponent);
            }
        }
    }
    private void Disctionary_image_XandY()
    {
        for (int i = 0; i < image.Length; i++)
        {
            _imageW.Add(imageName[i], image[i].bounds.size.x);
            _imageH.Add(imageName[i], image[i].bounds.size.y);
        }
    }

    public void AddImageObj(int Num)
    {
        
        for (int i = 0; i < Num; i++)
        {
            GameObject  imageObjectPrefab = Instantiate(imageObjPrefab, Vector3.zero, Quaternion.identity);
            imageObjectPrefab.transform.parent = gameObject.transform;
            RectTransform rectTransform = imageObjectPrefab.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
        }
            
    }
    public void AddImage(string image_name,int image_num,int image_ObjNum, float Zoom_w = 1, float Zoom_h = 1, float offset_x = 0, float offset_y = 0)
    {
        imageList[image_ObjNum].sprite = image[image_num];
        rTList[image_ObjNum].sizeDelta = new Vector2(_imageW[image_name] * Zoom_w, _imageH[image_name] * Zoom_h);
        if (offset_x != 0 || offset_y != 0)
        {
            rTList[image_ObjNum].anchoredPosition3D = new Vector2(offset_x, offset_y);
        }
    }
    public void ImageActive(bool Active,int image_ObjNum)
    {
        transform.GetChild(image_ObjNum).gameObject.SetActive(Active);
    }
    public void Fade(int image_ObjNum,float timedelay, bool def = true)
    {
        StartCoroutine(color_A(image_ObjNum,timedelay, def));
    }
    public IEnumerator color_A(int image_ObjNum,float timedelay, bool def = true)
    {
        var timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            var myA = def ? Mathf.Lerp(0f, 1f, curve.Evaluate(timer / timedelay)) : Mathf.Lerp(1f, 0f, curve.Evaluate(timer / timedelay));
            imageList[image_ObjNum].color = new(imageList[image_ObjNum].color.r, imageList[image_ObjNum].color.g, imageList[image_ObjNum].color.b, myA);
            yield return null;
        }
    }
    public void Zoom(int image_ObjNum,float timedelay, float xzoom = 1f, float yzoom = 1f)
    {
        StartCoroutine(_Zoom(image_ObjNum,timedelay, xzoom, yzoom));
    }

    private IEnumerator _Zoom(int image_ObjNum,float timedelay,float xzoom, float yzoom)
    {
        float timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float X_zoom = Mathf.Lerp(rTList[image_ObjNum].localScale.x, xzoom, curve.Evaluate(timer / timedelay));
            float Y_zoom = Mathf.Lerp(rTList[image_ObjNum].localScale.y, yzoom, curve.Evaluate(timer / timedelay));
            rTList[image_ObjNum].localScale = new Vector2(X_zoom, Y_zoom);
            yield return null;
        }
    }
    public void Offset(int image_ObjNum,float timedelay, float xoffset = 0f, float yoffset = 0f)
    {
        StartCoroutine(_Offset(image_ObjNum,timedelay, xoffset, yoffset));
    }
    public IEnumerator _Offset(int image_ObjNum,float timedelay, float xoffset = 0f, float yoffset = 0f)
    {
        var anchoredPosition = rTList[image_ObjNum].anchoredPosition;
        var x_offset = anchoredPosition.x;
        var y_offset = anchoredPosition.y;
        var timer = 0f;
        while (timer < timedelay)
        {
            timer += Time.deltaTime;
            float my_x = Mathf.Lerp(x_offset, x_offset + xoffset, curve.Evaluate(timer / timedelay));
            float my_y = Mathf.Lerp(y_offset, y_offset + yoffset, curve.Evaluate(timer / timedelay));
            rTList[image_ObjNum].anchoredPosition3D = new Vector2(my_x, my_y);
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
                yield return StartCoroutine(_Zoom(0,0.05f, 1.2f, 1.2f));
                yield return StartCoroutine(_Zoom(0,0.05f, 1f, 1f));
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
