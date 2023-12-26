using Events;
using UnityEngine;
public class PlayerVar : Singleton<PlayerVar>
{
    public Damageable damageable;
    public PMagic pMagic;
    public ShuDyeingVar shuDyeingVar;

    private void Start()
    {
        InvokeRepeating("D1", 0f, 1.07f);
        InvokeRepeating("D2", 0f, 1.24f);
        InvokeRepeating("D3", 0f, 2.53f);
        InvokeRepeating("D4", 0f, 0.53f);
    }
    
    private void D1()
    {
        if (shuDyeingVar.filthyVar >= 50 && damageable.IsAlive)
        {
            damageable.Hit(10, new Vector2(0, 0), false);
            CharacterEvents.characterShuDyeing.Invoke(gameObject, "¤¤¬r");
        }
    }
    private void D2()
    {
        if (shuDyeingVar.filthyVar >= 70 && damageable.IsAlive)
        {
            damageable.Hit(20, new Vector2(0, 0), false);
            CharacterEvents.characterShuDyeing.Invoke(gameObject, "°IºÜ");
        }
    }private void D3()
    {
        if (shuDyeingVar.filthyVar >= 85 && damageable.IsAlive)
        {
            damageable.Hit(25, new Vector2(0, 0), false);
            CharacterEvents.characterShuDyeing.Invoke(gameObject, "µh­W");
        }
    }private void D4()
    {
        if (shuDyeingVar.filthyVar >= 93 && damageable.IsAlive)
        {
            damageable.Hit(5, new Vector2(0, 0), false);
            CharacterEvents.characterShuDyeing.Invoke(gameObject, "¼RÁn¤OºÜ");
        }
    }
}
