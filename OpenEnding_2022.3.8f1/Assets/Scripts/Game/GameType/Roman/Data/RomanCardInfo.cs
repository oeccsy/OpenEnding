using UnityEngine;

namespace Game.GameType.Roman.Data
{
    [CreateAssetMenu(fileName = "CardInfoSO", menuName = "Scriptable Object/Roman/CardInfoSO")]
    public class RomanCardInfo : ScriptableObject
    {
        public CardType cardType = CardType.None;
        public string cardName;
        [TextArea]
        public string cardDesc;
    }
}