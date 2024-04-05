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
        }
        
        protected override IEnumerator Start()
        {
            foreach (var polygon in _polygons) polygon.meshRenderer.material.DOFade(0, 0);
            yield return base.Start();
            yield return ShowPolygons();
        }
        
        private IEnumerator ShowPolygons()
        {
            for (int i = 0; i < 3; i++)
            {
                Polygon polygon = _polygons[i];
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;
                
                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 0.2f))
                    .Join(polygonTransform.DOScale(1f, 0.2f).From(0f).SetEase(Ease.InCirc))
                    .Join(polygonTransform.DOLocalMoveY(endPos.y, 0.3f).From(endPos.y - 0.5f).SetEase(Ease.OutCirc))
                    .AppendCallback(() => _circleARigidBody.simulated = true)
                    .AppendCallback(() => _circleBRigidBody.simulated = true);
            }

            yield return new WaitForSeconds(2.5f);

            _circleARigidBody.simulated = false;
            _circleBRigidBody.simulated = false;
            
            for (int i = 3; i < 6; i++)
            {
                Polygon polygon = _polygons[i];
                Transform polygonTransform = polygon.transform;
                Vector3 endPos = polygonTransform.localPosition;
                
                Sequence sequence = DOTween.Sequence();

                sequence
                    .Append(polygon.meshRenderer.material.DOFade(1, 1f))
                    .Join(polygonTransform.DOScale(1f, 1f).From(0f).SetEase(Ease.OutCirc))
                    .Join(polygonTransform.DOLocalMoveY(endPos.y, 1f).From(endPos.y - 0.5f).SetEase(Ease.OutCirc))
                    .AppendInterval(0.5f)
                    .Append(polygonTransform.DOPunchScale(Vector3.one * 0.1f, 0.25f).SetEase(Ease.InCirc));
            }
            
            yield return null;
        }
        
        public override IEnumerator Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}