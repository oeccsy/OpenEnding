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
    public class Artwork : RomanCard
    {
        private List<Polygon> _polygons = new List<Polygon>();
        private Sequence _showPolygonSequence = null;
        
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.Artwork;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_Artwork");
            cardInfoUI.RefreshUI(cardInfo);
            
            _polygons = GetComponentsInChildren<Polygon>().ToList();
            
            foreach (var polygon in _polygons) polygon.SetAlpha(0f);
        }
        
        protected override IEnumerator ShowPolygons()
        {
            if (_showPolygonSequence == null)
            {
                _showPolygonSequence = CreateShowSequence();
            }
            else
            {
                _showPolygonSequence.Restart();
            }
            
            yield return _showPolygonSequence.WaitForCompletion();
        }

        private Sequence CreateShowSequence()
        {
            Sequence mainSequence = DOTween.Sequence().SetAutoKill(false);
            
            var showOrder = new List<Polygon>(_polygons);
            showOrder.Reverse();
            
            for (int i = 0; i<showOrder.Count; i++)
            {
                Polygon polygon = showOrder[i];
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;
                
                Sequence subSequence = DOTween.Sequence();
            
                subSequence
                    .AppendCallback(()=> polygonTransform.localScale = Vector3.one)
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOLocalMove(endPos, 0.2f).From(endPos + Vector3.up).SetEase(Ease.InCirc))
                    .Append(polygonTransform.DOPunchPosition(Vector3.up * 0.2f, 0.2f, 1).SetEase(Ease.OutCirc))
                    .Join(polygonTransform.DOPunchRotation(Vector3.forward * 10f, 0.2f, 1).SetEase(Ease.OutCirc));
                    
                mainSequence
                    .Insert(i * 0.2f, subSequence);
            }

            return mainSequence;
        }
        
        public override IEnumerator Hide()
        {
            Sequence sequence = null;
            
            foreach (var polygon in _polygons)
            {
                Transform polygonTransform = polygon.transform;
                
                sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(0, 0.2f))
                    .Join(polygonTransform.DOScale(0f, 0.2f).SetEase(Ease.InCirc));
            }

            yield return sequence.WaitForCompletion();
        }
    }
}