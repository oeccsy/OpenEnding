using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Polygon;
using DG.Tweening;
using Game.GameType.Roman.ClientSide.CardBase;
using Game.GameType.Roman.Data;
using UnityEngine;

namespace Game.GameType.Roman.ClientSide.Card
{
    public class A : RomanCard
    {
        private List<Polygon> _polygons = new List<Polygon>();
        
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.A;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_A");
            cardInfoUI.RefreshUI(cardInfo);

            _polygons = GetComponentsInChildren<Polygon>().ToList();
        }

        private void Start()
        {
            foreach (var polygon in _polygons)
            {
                polygon.Sides = Random.Range(5, 7);

                switch (polygon.Sides)
                {
                    case 5:
                        polygon.SetColor(Color.yellow);
                        break;
                    case 6:
                        polygon.SetColor(Color.green);
                        break;
                }
                polygon.transform.localScale = Vector3.one * 0.3f;

                StartCoroutine(PolygonAnimRoutine(polygon));
            }
        }

        private IEnumerator PolygonAnimRoutine(Polygon polygon)
        {
            while (true)
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        polygon.transform.DORotate(Vector3.forward * 720, 2f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutCirc);
                        yield return new WaitForSecondsRealtime(Random.Range(2f, 3f));
                        break;
                    case 1:
                        polygon.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutCirc);
                        yield return new WaitForSecondsRealtime(Random.Range(1f, 2f));
                        polygon.transform.DOScale(Vector3.one * 0.3f, 1f).SetEase(Ease.OutCirc);
                        yield return new WaitForSecondsRealtime(Random.Range(1f, 2f));
                        break;
                }
            }
        }
    }
}