namespace Game.GameType.Roman.ClientSide
{
    public class RomanGameState : GameState
    {
        public GameStep curStep = GameStep.InitGame;
        
        public ColorPalette.ColorName curPlayer = ColorPalette.ColorName.DeviceDefault;
        public ColorPalette.ColorName winner = ColorPalette.ColorName.DeviceDefault;

        public void SynchronizeStep(GameStep step)
        {
            curStep = step;
        }
        
        public void SynchronizeCurPlayer(ColorPalette.ColorName player)
        {
            curPlayer = player;
        }
        
        public void SynchronizeWinner(ColorPalette.ColorName winner)
        {
            this.winner = winner;
        }
    }
}