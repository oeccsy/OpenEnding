using Game.GameType.Roman.ClientSide;
using Game.GameType.Roman.ServerSide.CardBase;
using Game.Manager.GameManage;


namespace Game.GameType.Roman.ServerSide.Card
{
    public class Sprout : RomanCard, IFlipAbility
    {
        public Sprout()
        {
            cardType = CardType.Sprout;
        }

        private ColorPalette.ColorName _firstPlayer = ColorPalette.ColorName.DeviceDefault;
        private int _flipCount = 0;
        
        public void FlipAbility()
        {
            if (_firstPlayer == ColorPalette.ColorName.DeviceDefault)
            {
                var gameState = GameManager.Instance.GameState as RomanGameState;
                _firstPlayer = gameState.curPlayer;
            }
            
            _flipCount++;

            if (_flipCount >= 10)
            {
                var gameMode = GameManager.Instance.GameMode as RomanGameMode;
                gameMode.Victory(_firstPlayer);
            }
        }

        public override void OnEnterField() {}
        public override void OnExitField() {}
    }
}