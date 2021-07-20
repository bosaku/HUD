using UnityEngine;

public class HudSymbol : MonoBehaviour
{
    private Hud _hud;
    public bool displaySymbol = true;

    private void Awake()
    {
        _hud = FindObjectOfType<Hud>();

        _hud.Bind(this);
    }
}