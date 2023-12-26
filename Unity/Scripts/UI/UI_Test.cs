using Events;
using TMPro;

public class UI_Test : Singleton<UI_Test>
{
    TextMeshProUGUI TM;

    public string text;
    protected override void Awake()
    {
        base.Awake();
        TM = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        TM.text = text;
    }

}
