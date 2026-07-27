using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string String_Color_Rarity(Rarity rare)
    {
        switch (rare)
        {
            case Rarity.Common:
                return "<color=#f0f6fe>";
            case Rarity.UnCommon:
                return "<color=#24f01d>";
            case Rarity.Rare:
                return "<color=#1d80f0>";
            case Rarity.Hero:
                return "<color=#8e1df0>";
            case Rarity.Legendary:
                return "<color=#dff01d>";
        }
        return "<color=#FFFFFF>";
    }
}
