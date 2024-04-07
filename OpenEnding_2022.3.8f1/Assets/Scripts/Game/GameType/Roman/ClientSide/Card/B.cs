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
        [SerializeField]
        private Transform _pivot;
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
            List<Polygon> targetPolygons = new List<Polygon>();
            targetPolygons.Add(_polygons[0]);
            targetPolygons.Add(_polygons[2]);

            foreach (var polygon in targetPolygons)
            {
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;

                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalMoveY(endPos.y, 0.5f).From(endPos.y - 0.5f).SetEase(Ease.OutCirc))
                    .Append(polygonTransform.DOLocalMoveY(endPos.y - 0.5f, 0.5f).SetEase(Ease.InCirc))
                    .Append(polygonTransform.DOLocalMoveY(endPos.y, 0.5f).SetEase(Ease.OutCirc));

            }

            yield return new WaitForSeconds(1f);
            
            Sequence rotateSequence = DOTween.Sequence();
                
            rotateSequence
                .Append(_pivot.DOLocalRotate(Vector3.forward * 90, 1f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutCirc))
                .Append(_pivot.DOLocalRotate(Vector3.forward * -90, 1f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutCirc));
            
            yield return new WaitForSeconds(1f);

            foreach (var polygon in targetPolygons)
            {
                Transform polygonTransform = polygon.transform;

                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygonTransform.DOLocalRotate(Vector3.forward * 360, 1f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutCirc));
            }

            targetPolygons.Clear();
            targetPolygons.Add(_polygons[1]);
            targetPolygons.Add(_polygons[3]);

            foreach (var polygon in targetPolygons)
            {
                Transform polygonTransform = polygon.transform;

                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.5f))
                    .Join(polygonTransform.DOScale(1f, 0.5f).From(0f).SetEase(Ease.InCirc));
            }
        }

        public override IEnumerator Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}