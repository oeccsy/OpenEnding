# OpenEnding
> ### 카드처럼 다루는 모바일 디바이스, 현장에서 즐기는 보드게임
```
OpenEnding은 코로나 이후로 잃어버린 오프라인 모임의 즐거움을 되찾기 위해 개발한 현장에서 즐기는 보드게임 입니다.
모바일 디바이스가 카드와 닮았다는 점에서 착안하여 디바이스의 자체를 카드처럼 뒤집는 플레이 방식을 구현하였습니다.
현장감을 강조하기 위해 외부 서버의 도움 없이 블루투스 통신으로 멀티플레이를 구현하였습니다.
```

<div align="center">
  <table>
    <tr>
      <th><img src="https://github.com/oeccsy/OpenEnding/assets/77562357/c41d5f4e-7e1c-423c-acdc-8dff069df78e" width="360px" height="640px"/></th>
      <th width="270px" height="600px"> => </th>
      <th><img src="https://github.com/oeccsy/OpenEnding/assets/77562357/374852f5-ceb2-4307-8b2d-56f471aa5224" width="360px" height="640px"/></th>
    </tr>
    <tr>
      <td align="center">▲ Open</td>
      <td align="center" width="270px"> </td>
      <td align="center">▲ Ending</td>
    </tr>
  </table>
</div>

<br>
<br>

<div align="center">
  <table>
    <tr>
      <th> 플랫폼 </th>
      <th> 장르 </th>
      <th> 개발 환경 </th>
      <th> 개발 인원 </th>
      <th> 특징 </th>
      <th> 시연 영상 </th>
    </tr>  
    <tr>
      <td align="center"> <code> Android </code> </td>
      <td align="center"> <code> 보드게임 </code> </td>
      <td align="center"> <code> Unity3D </code> <code> C# </code> </td>
      <td align="center"> <code> 본인 1명 </code> </td>
      <td align="center"> <code> 멀티플레이 </code> </td>
      <td align="center"> <a href="https://youtu.be/1owshpb0_mI"> <code> YouTube </code> </a> </td>
    </tr>
  </table>
</div>


<br>
<br>

## 멀티플레이
- 모임 현장에서 플레이하는 취지와 잘 어울리는 `Bluetooth LE`로 구현했습니다.
- `1:n`으로 통신하는 `클라이언트-서버` 구조로 구현했습니다.
- `RPC`를 적용하여 동기화를 구현했습니다.
- 서버 유지의 문제가 해결되어 `서비스 종료` 개념이 없습니다.

![Game Architecture 1](https://github.com/oeccsy/OpenEnding/assets/77562357/d45a9f66-0124-4a36-9eb8-84cb657ba195)  

<br>
<br>

## 카드를 뒤집는 행위
- 자이로센서를 이용하여 구현했습니다.
- 카드처럼 뒤집는 행위를 통해 오프라인 보드게임의 물리적인 경험을 제공합니다.
- 이러한 플레이 방식은 유저의 시야가 스마트폰 화면 밖으로 확장되는 기회가 되었습니다.

<div align="center">
   <table>
     <tr>
       <th><img src="https://github.com/oeccsy/OpenEnding/assets/77562357/1c18b1e0-6225-4094-b92b-6cc606f8cbac" width="360px" height="270px"/></th>
       <th width="100px" height="400px"> vs </th>
       <th><img src="https://github.com/oeccsy/OpenEnding/assets/77562357/d9a63c5c-5309-45c0-a94b-d4aa9d282320" width="360px" height="316px"/></th>
     </tr>
     <tr>
       <td align="center">▲ OpenEnding </td>
       <td align="center" width="100px"> </td>
       <td align="center">▲ 일반적인 모바일 파티게임</td>
     </tr>
   </table>
</div>

<br>
<br>

## 자이가르니크 효과
> 마치지 못한 일을 쉽게 마음 속에서 지우지 못하는 현상

- 자이가르니크 효과는 이커머스 분야에서 서비스의 `리텐션`을 높이는데 자주 사용합니다.
- 도전과 포기 사이의 딜레마를 통해 게임 규칙에 적용을 시도했습니다.
- 포기한 경우 `GrayScale`로 렌더링하는 연출을 구현했습니다.

<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/23e74792-8d54-431f-b0fa-e719991f1435"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/1c741cd1-fb22-478a-9f26-9625b39db056"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/a54729a3-9de5-4451-8452-2be2ef9d36d2"  width="270px" height="600px"/>
<br>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/534fd939-7c19-4164-b55e-0d083f524c8b"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/3c9ee3b7-3d63-4860-b052-0e901da84dc6"  width="270px" height="600px"/>
<img src="https://github.com/oeccsy/OpenEnding/assets/77562357/00b9028f-4ef9-42d2-b731-4aba4ecf6548"  width="270px" height="600px"/>

