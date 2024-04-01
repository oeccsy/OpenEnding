using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class BackgroundUI : MonoBehaviour
    {
        private static Image backgroundImage;

        private void Awake()
        {
            backgroundImage = GetComponent<Image>();
        }

        public static void SetBackgroundColor(Color color, float duration = 2f)
        {
            if (backgroundImage == null) return;
        
            backgroundImage.DOColor(color, 2f);
        }
    }
}
