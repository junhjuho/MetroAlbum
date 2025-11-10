using ARLocation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class Datatest : MonoBehaviour
{
    public ARLocationProvider al; // 사용자의 실시간 GPS 위치 정보를 제공하는 컴포넌트
    public PlaceAtLocation pa; // 목표 1 위치 정보를 담고 있는 컴포넌트
    public PlaceAtLocation pa2; // 목표 2 위치 정보를 담고 있는 컴포넌트
    public Text text1;
    public Text text2;

    public GameObject go1;
    public GameObject go2;
    // Start is called before the first frame update
    void Start()
    {
        al.OnLocationUpdated.AddListener((location) => {
            OnLocationUpdated(location);
        });
        // 이벤트 리스너 등록
        // OnLocationUpdated -> 나의 GPS 위치가 갱신될 때마다 발생하는 이벤트
        // location -> 이벤트가 전달하는 매개변수(현재 위치 정보)
        // GPS가 갱신될 때마다 현재 위치 정보(location) 매개변수를 보내서 OnLocationUpdated 메서드를 호출한다
    }
    void OnLocationUpdated(Location location) // 목표 위치, 현재 나의 위치를 계산해서 10m 이내일 때 오브젝트 활성화
    {

        float distance1 = (float)Location.HorizontalDistance(location, pa.Location); // 목표 1 까지 남은 거리
        float distance2 = (float)Location.HorizontalDistance(location, pa2.Location); // 목표 2 까지 남은 거리

        //text1.text = $"남은 거리 : {distance1:F1}m";
        //text2.text = $"남은 거리 : {distance2:F1}m";

        go1.SetActive(distance1 <= 10f); // 10m 이하일 시 오브젝트 활성화
        go2.SetActive(distance2 <= 10f);

        SortUIByDistance(distance1, distance2);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    void SortUIByDistance(float distance1, float distance2)
    {
        if (!go1.activeSelf || !go2.activeSelf) return; // 둘 중 하나라도 비활성화면 리턴
        // 둘 다 활성화되어 있을 때만 정렬
        if (distance1 < distance2)
        {
            go1.transform.SetSiblingIndex(0);
            go2.transform.SetSiblingIndex(1);
        }
        else
        {
            go2.transform.SetSiblingIndex(0);
            go1.transform.SetSiblingIndex(1);
        }
    }    
}
