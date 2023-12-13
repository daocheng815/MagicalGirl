using UnityEngine;
public class EnemySearchToPlayers : MonoBehaviour
{
    [SerializeField]private GameObject Player;
    [SerializeField]private bool isOnPlayer;
    [SerializeField]private bool enableDrawRay; 
    public Vector3 PlayerWorldLocation => Player.transform.position;
    public float distance;
    public Vector3 direction;
    private void Awake()
    {
        //������Ҭ����a������
        Player = GameObject.FindWithTag("Player");
        isOnPlayer = Player != null ?  true : false;
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(PlayerWorldLocation, transform.position);
        direction = (PlayerWorldLocation - transform.position).normalized;
        if(enableDrawRay)
            Debug.DrawRay(transform.position,PlayerWorldLocation - transform.position, Color.red);
    }
}
