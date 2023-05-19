using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ServerScene : MonoBehaviour
{
    private void Start()
    {
        // 현재 컴퓨터의 호스트 이름을 가져옵니다.
        string hostname = Dns.GetHostName();

        // 호스트 이름을 IP 주소로 변환합니다.
        IPAddress[] ips = Dns.GetHostAddresses(hostname);

        // 로그로 IP 주소를 출력합니다.
        foreach (IPAddress ip in ips)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                Debug.Log("IP Address: " + ip.ToString());
                Debug.Log($"HostName: {hostname}");
            }
        }
    }
}
