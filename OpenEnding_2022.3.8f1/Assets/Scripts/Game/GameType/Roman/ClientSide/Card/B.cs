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
        
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.B;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_B");
            cardInfoUI.RefreshUI(cardInfo);
            
            _polygons = GetComponentsInChildren<Polygon>().ToList();
        }
        
        protected override IEnumerator Start()
        {
            foreach (var polygon in _polygons) polygon.meshRenderer.material.DOFade(0, 0);
            
            yield return base.Start();
            yield return ShowPolygons();
        }
        
        private IEnumerator ShowPolygons()
        {
            var showOrder = new List<Polygon>(_polygons);
            showOrder.Reverse();
            
            foreach (var polygon in showOrder)
            {
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;

                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalMoveY(endPos.y, 0.5f).From(endPos.y - 0.5f).SetEase(Ease.OutCirc))
                    .Append(polygonTransform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.InCirc))
                    .Append(polygonTransform.DOLocalMoveY(endPos.y, 0.5f).SetEase(Ease.OutCirc));

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(1f);

            Sequence rotSequence = DOTween.Sequence();

            rotSequence
                .AppendCallback(() => _polygons[1].transform.SetParent(_polygons[0].transform))
                .Append(_polygons[0].transform.DORotate(Vector3.forward * 45, 1f).SetEase(Ease.InOutCirc))
                .Join(_polygons[0].transform.DOMoveX(_polygons[0].transform.position.x * -1, 1f).SetEase(Ease.InOutCirc))
                .Append(_polygons[0].transform.DORotate(Vector3.zero, 1f).SetEase(Ease.InOutCirc))
                .Join(_polygons[0].transform.DOMoveX(0, 1f).SetEase(Ease.InOutCirc));

            yield return new WaitForSeconds(2f);
            
            foreach (var polygon in _polygons)
            {
                Tween radiusTween = DOTween.To(() => polygon.Radius, r => polygon.Radius = r, 1, 1f);
                
                Sequence sequence = DOTween.Sequence();
                
                sequence
                    .Append(radiusTween.SetEase(Ease.InOutCirc))
                    .Join(polygon.transform.DOMoveY(1.4f, 1f).SetEase(Ease.InOutCirc));
            }
        }

        public override IEnumerator Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}