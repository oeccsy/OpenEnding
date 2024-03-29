namespace Game.GameType.Roman.ClientSide
{
    public class RomanGameState : GameState
    {
        public GameStep curStep = GameStep.InitGame;
        
        public ColorPalette.ColorName curPlayer = ColorPalette.ColorName.DeviceDefault;
        public ColorPalette.ColorName winner = ColorPalette.ColorName.DeviceDefault;
    }
}