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
    public class B : RomanCard
    {
        private List<Polygon> _polygons = new List<Polygon>();
        private Sequence _showPolygonSequence = null;
        
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.B;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_B");
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

            for (int i = 0; i < showOrder.Count; i++)
            {
                Polygon polygon = showOrder[i];
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;

                Sequence subSequence = DOTween.Sequence();

                subSequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalMove(endPos, 0.5f).From(endPos - Vector3.up * 0.5f).SetEase(Ease.OutCirc))
                    .Append(polygonTransform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.InCirc))
                    .Append(polygonTransform.DOLocalMoveY(endPos.y, 0.5f).SetEase(Ease.OutCirc));

                mainSequence
                    .Insert(i * 0.5f, subSequence);
            }

            Sequence rotSequence = DOTween.Sequence();

            rotSequence
                .AppendCallback(() => _polygons[1].transform.SetParent(_polygons[0].transform))
                .Append(_polygons[0].transform.DORotate(Vector3.forward * 45, 1f).SetEase(Ease.InOutCirc))
                .Join(_polygons[0].transform.DOMoveX(_polygons[0].transform.position.x * -1, 1f).SetEase(Ease.InOutCirc))
                .Append(_polygons[0].transform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutCirc))
                .Join(_polygons[0].transform.DOMoveX(0, 1f).SetEase(Ease.InOutCirc))
                .AppendCallback(() => _polygons[1].transform.SetParent(_polygons[0].transform.parent));

            mainSequence
                .Append(rotSequence)
                .AppendInterval(0f);
            
            foreach (var polygon in _polygons)
            {
                Tween radiusTween = DOTween.To(() => polygon.Radius, r => polygon.Radius = r, 1, 1f);
                
                Sequence subSequence = DOTween.Sequence();
                
                subSequence
                    .Append(radiusTween.SetEase(Ease.InOutCirc))
                    .Join(polygon.transform.DOMoveY(1.4f, 1f).SetEase(Ease.InOutCirc));

                mainSequence
                    .Join(subSequence);
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