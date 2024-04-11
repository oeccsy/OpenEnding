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
            
            foreach (var polygon in _polygons) polygon.SetAlpha(0f);
        }

        protected override IEnumerator ShowPolygons()
        {
            foreach (var polygon in _polygons)
            {
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;
                Vector3 endRot = polygonTransform.localRotation.eulerAngles;
                int endSides = polygon.Sides;
                polygon.Sides = 100;
                
                Sequence sequence = DOTween.Sequence();
                
                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalMoveY(2.5f, 0.3f).SetEase(Ease.OutCirc))
                    
                    .Append(polygonTransform.DOLocalMoveY(0.5f, 0.5f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InCirc))
                    .AppendCallback(()=> polygon.Sides = 7 + Random.Range(0, 2))
                    .Append(polygonTransform.DOLocalMoveY(2.4f, 0.5f).SetEase(Ease.OutCirc))
                    .Join(polygonTransform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc))
                    
                    .Append(polygonTransform.DOLocalMoveY(0.5f, 0.5f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InCirc))
                    .AppendCallback(()=> polygon.Sides = 4 + Random.Range(0, 2))
                    .Append(polygonTransform.DOLocalMoveY(2.3f, 0.5f).SetEase(Ease.OutCirc))
                    .Join(polygonTransform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutCirc))
                    
                    .Append(polygonTransform.DOLocalMoveY(0.5f, 0.5f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalRotate(Vector3.forward * 180, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.InCirc))
                    .AppendCallback(()=> polygon.Sides = endSides)
                    .Append(polygonTransform.DOLocalMove(endPos, 0.5f).SetEase(Ease.OutCirc))
                    .Join(polygonTransform.DORotate(endRot, 0.5f, RotateMode.FastBeyond360).SetEase(Ease.OutCirc));
            }

            yield return null;
        }

        public override IEnumerator Hide()
        {
            foreach (var polygon in _polygons)
            {
                Transform polygonTransform = polygon.transform;
                
                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(0, 0.2f))
                    .Join(polygonTransform.DOScale(0f, 0.2f).SetEase(Ease.InCirc));
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}