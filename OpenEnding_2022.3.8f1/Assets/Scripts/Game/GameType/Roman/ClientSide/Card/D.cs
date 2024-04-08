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
    public class D : RomanCard
    {
        private List<Polygon> _polygons = new List<Polygon>();
        
        private Polygon _circleA;
        private Polygon _circleB;
        private Polygon _land;
        
        private Polygon _stem;
        private Polygon _leafA;
        private Polygon _leafB;
        
        private Rigidbody2D _circleARigidBody;
        private Rigidbody2D _circleBRigidBody;
        
        protected override void Awake()
        {
            base.Awake();
            cardType = CardType.D;
            
            RomanCardInfo cardInfo = Resources.Load<RomanCardInfo>("ScriptableObject/Roman/CardInfoSO_D");
            cardInfoUI.RefreshUI(cardInfo);
            
            _polygons = GetComponentsInChildren<Polygon>().ToList();
            
            _circleA = _polygons[0];
            _circleB = _polygons[1];
            _land = _polygons[2];
            _stem = _polygons[3];
            _leafA = _polygons[4];
            _leafB = _polygons[5];
            
            _circleARigidBody = _circleA.gameObject.GetComponent<Rigidbody2D>();
            _circleBRigidBody = _circleB.gameObject.GetComponent<Rigidbody2D>();
            
            foreach (var polygon in _polygons) polygon.SetAlpha(0f);
        }
        
        protected override IEnumerator Start()
        {
            yield return base.Start();
            yield return ShowPolygons();
        }
        
        private IEnumerator ShowPolygons()
        {
            Transform circleATransform = _circleA.transform;
            Vector3 circleAEndPos = circleATransform.localPosition;
                
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(_circleA.meshRenderer.material.DOFade(1, 0.2f))
                .Join(circleATransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                .Join(circleATransform.DOLocalMoveY(circleAEndPos.y, 0.3f).From(circleAEndPos.y - 0.5f).SetEase(Ease.OutCirc))
                .AppendCallback(() => _circleARigidBody.simulated = true)
                .AppendInterval(2.5f)
                .AppendCallback(() => _circleARigidBody.simulated = false);
            
            Transform circleBTransform = _circleB.transform;
            Vector3 circleBEndPos = circleBTransform.localPosition;
                
            sequence = DOTween.Sequence();

            sequence
                .Append(_circleB.meshRenderer.material.DOFade(1, 0.2f))
                .Join(circleBTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                .Join(circleBTransform.DOLocalMoveY(circleBEndPos.y, 0.3f).From(circleBEndPos.y - 0.5f).SetEase(Ease.OutCirc))
                .AppendCallback(() => _circleBRigidBody.simulated = true)
                .AppendInterval(2.5f)
                .AppendCallback(() => _circleBRigidBody.simulated = false);
            
            Transform landTransform = _land.transform;
            Vector3 landEndPos = landTransform.localPosition;
            
            sequence = DOTween.Sequence();
            
            sequence
                .Append(_land.meshRenderer.material.DOFade(1, 0.2f))
                .Join(landTransform.DOLocalMoveY(landTransform.position.y, 0.2f).From(landTransform.position.y + 1f).SetEase(Ease.InCirc))
                .Append(landTransform.DOPunchPosition(Vector3.up * 0.2f, 0.2f, 1).SetEase(Ease.OutCirc))
                .Join(landTransform.DOPunchRotation(Vector3.forward * 10f, 0.2f, 1).SetEase(Ease.OutCirc))
                .AppendInterval(2f)
                .Append(landTransform.DOPunchScale(Vector3.one * 0.1f, 0.5f));

            yield return new WaitForSeconds(3f);

            for (int i = 3; i < 6; i++)
            {
                Polygon targetPolygon = _polygons[i];
                Transform targetPolygonTransform = targetPolygon.transform;
                Vector3 endPos = targetPolygonTransform.position;
                
                sequence = DOTween.Sequence();

                sequence
                    .Append(targetPolygon.meshRenderer.material.DOFade(1, 0.5f))
                    .Join(targetPolygonTransform.DOScale(Vector3.one, 0.5f).From(0)).SetEase(Ease.InOutCirc)
                    .Join(targetPolygonTransform.DOLocalMoveY(endPos.y, 0.5f).From(endPos.y - 0.5f).SetEase(Ease.InOutCirc));
            }
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