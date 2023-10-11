using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    // Start is called before the first frame update
    void Start()
    {
        orgtransform = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable) 
        {
            bool wasHealth = damageable.Heal(healthRestore);
            if (wasHealth)
                Destroy(gameObject);
        }
    }
    private float updoneSpeed = 0.5f;
    private float updoneVal = 0.35f;
    private bool IsUp = true;
    private Vector3 orgtransform;

    private void Update()
    {
        // 以歐拉角表示旋轉
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;

        if (IsUp)
        {
            transform.position = transform.position + new Vector3(0, updoneSpeed * Time.deltaTime, 0);
            IsUp = transform.position.y < orgtransform.y + updoneVal;
        }
        
        else if (!IsUp)
        {
            transform.position = transform.position + new Vector3(0, -updoneSpeed * Time.deltaTime, 0);
            IsUp = transform.position.y < orgtransform.y - updoneVal;
        }
        
        //Debug.Log(IsUp);
    }
}
