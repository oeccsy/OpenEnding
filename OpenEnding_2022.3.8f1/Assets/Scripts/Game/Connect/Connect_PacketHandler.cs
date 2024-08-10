using System;
using System.Collections.Generic;
using Game.Manager.GameManage;
using Game.Manager.NetworkManage;

public class Connect_PacketHandler : PacketHandler
{
    private Connect_Scene ConnectScene => Connect_Scene.Instance;
    private GameFlow GameFlow => GameManager.Instance.GameFlow;

    protected override void Awake()
    {
        base.Awake();

        _instanceDict = new Dictionary<Type, object>
        {
            {typeof(Connect_Scene), ConnectScene},
            {typeof(GameFlow), GameFlow}
        };
    }
}
