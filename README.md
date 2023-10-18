# OpenEnding

> ### "카드처럼 다루는 모바일 디바이스"

```
OpenEnding은 코로나 이후로 잃어버린 오프라인 모임의 즐거움을 되찾기 위해 개발한 현장에서 즐기는 보드게임 입니다.
모바일 디바이스가 카드와 닮았다는 점에서 착안하여 디바이스의 자체를 카드처럼 뒤집는 플레이 방식을 구현하였습니다.
현장감을 강조하기 위해 외부 서버의 도움 없이 블루투스 통신으로 멀티플레이를 구현하였습니다.
```
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/23e74792-8d54-431f-b0fa-e719991f1435"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/1c741cd1-fb22-478a-9f26-9625b39db056"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/a54729a3-9de5-4451-8452-2be2ef9d36d2"  width="270px" height="600px"/>

- 게임 플레이 영상 : https://youtu.be/1owshpb0_mI

   

## 멀티플레이
```
게임은 모든 플레이어가 협동하는 멀티플레이 게임 입니다.
유저들이 나눠받은 각각의 카드는 게임의 주인공인 거북이가 이루고 싶은 목표들 입니다.
협동을 통해 최대한 많은 목표를 이뤄내야 합니다.
블루투스 통신을 통해 구현한 멀티플레이 방식은, 현장의 모임을 강조하면서,
외부 서버의 도움이 없기 때문에 서비스 종료의 개념이 없습니다. 이런 점은 보드게임 고유의 성질과 닮아있기도 합니다.
```

![Game Architecture 1](https://github.com/oeccsy/OpenEnding/assets/77562357/d45a9f66-0124-4a36-9eb8-84cb657ba195)



## 카드를 뒤집는 행위
```
모바일 디바이스는 카드의 역할을 수행합니다.
자이로센서를 이용하여 구현한 디바이스를 뒤집는 플레이 방식은 오프라인의 보드게임과 같은 경험을 제공합니다.
모바일 디바이스가 단순히 납작하게 생겼다는 유사성 뿐만 아니라, 카드처럼 뒤집는 행위를 부여할 수 있다는 점에서
실제로 카드게임을 하는 듯한 느낌을 줍니다. 또한 이를 통해 화면만을 들여다 보는 게임에서 시야가 확장된 게임 방식을 제공합니다. 
```



## 자이가르니크 효과
> 마치지 못한 일을 쉽게 마음 속에서 지우지 못하는 현상

<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/534fd939-7c19-4164-b55e-0d083f524c8b"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/3c9ee3b7-3d63-4860-b052-0e901da84dc6"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/00b9028f-4ef9-42d2-b731-4aba4ecf6548"  width="270px" height="600px"/>

