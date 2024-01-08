using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 敵人搜尋玩家
/// </summary>
public class SearchToPlayers : MonoBehaviour
{
    private GameObject _player;
    private bool _isOnPlayer;
    [SerializeField]private bool enableDrawRay; 
    public Vector3 PlayerWorldLocation => GetPlayerPos();
    public float distance;
    public Vector3 direction;

    public bool distanceBool = true;
    public bool distanceBools = true;
    
    private bool _pl;
    private bool _ml;
    private bool _c;
    public Vector3 playerPos => GetPlayerPos();

    public Vector3 GetPlayerPos()
    {
        return _player.transform.position;
    }
    public Vector3 GetPlayerScale()
    {
        return _player.transform.localScale;
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }
    public Vector3 GetScale()
    {
        return transform.localScale;
    }
    
    private void Awake()
    {
        //獲取標籤為玩家的物件
        _player = GameObject.FindWithTag("Player");
        _isOnPlayer = _player != null ?  true : false;
    }
    private void Update()
    {
        var myPos = GetPos();
        var myScale = GetScale();
        var playPos = GetPlayerPos();
        var playScale = GetPlayerScale();
        
        _c = playPos.x - myPos.x >= 0;
        _pl = playScale.x >= 1;
        _ml = myScale.x >= 1 ;
        
        DistanceBool(1f);
        
        distance = Vector3.Distance(playPos, myPos);
        direction = (playPos - myPos).normalized;
        if(enableDrawRay)
            Debug.DrawRay(myPos,playPos - myPos, Color.red);
    }
    
    private int _bl = 0;
    private int _bi = 0;
    private float _distanceDelayTime;
    void DistanceBool(float delayTime)
    {
        distanceBools = (_ml && _pl && _c) || (!_ml && !_pl && !_c) || (!_ml && _pl && !_c) || (_ml && !_pl && _c);
        _bl += distanceBools ? 1 : 0;
        _bi++;
        _distanceDelayTime += Time.deltaTime;
        if (_distanceDelayTime >= delayTime)
        {
            distanceBool = _bl >= _bi/2;
            _bl = 0;
            _bi = 0;
        }
    }
}
