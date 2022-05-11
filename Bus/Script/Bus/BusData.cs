using UnityEngine;
using Proyecto26;
/// <summary>
/// 公車資料結構
/// </summary>
[System.Serializable]
public class BusData
{
    public string PlateNumb;    //車牌號碼
    
    public RouteNameData RouteName;

    [System.Serializable]
    public class RouteNameData      
    {
        public string Zh_tw;    //中文路線名稱
        public RouteNameData(string zh_tw) => Zh_tw = zh_tw;
    }
    public string NextBusTime;  //下一班車到站時間
    public bool IsLastBus;  //是否為末班車
    public int StopStatus;  //車輛狀況[0:'正常',1:'尚未發車',2:'交管不停靠',3:'末班車已過',4:'今日未營運]

    public int BusStatus;   //行車狀況 : [0:'正常',1:'車禍',2:'故障',3:'塞車',4:'緊急求援',5:'加油',90:'不明',91:'去回不明',98:'偏移路線',99:'非營運狀態',100:'客滿',101:'包車出租',255:'未知']
    public int A2EventType;     //進站離站 : [0:'離站',1:'進站']

    public bool SubRoutes;      //是否有附屬路線資料
    public int Direction;       //去返程 : [0:'去程',1:'返程',2:'迴圈',255:'未知']
    public int BusRouteType;    //公車路線類別 : [11:'市區公車',12:'公路客運',13:'國道客運',14:'接駁車']
    public string DepartureStopNameZh;      //起始站
    public string DestinationStopNameZh;    //終點站
    
    public StopsData[] Stops;     //站牌結構
    
    [System.Serializable]
    public class StopsData
    {
        public StopNameData StopName;

        [System.Serializable]
        public class StopNameData
        {
            public string Zh_tw;    //中文路線名稱
            public StopNameData(string zh_tw) => Zh_tw = zh_tw;
        }
    }

    public BusData(string plateNumb,
                   RouteNameData routeName,
                   string nextBusTime,
                   bool isLastBus,
                   int stopstatus,
                   int busStatus,
                   int a2EventType,
                   bool subRoutes,
                   int direction,
                   int busRouteType,
                   string departureStopNameZh,
                   string destinationStopNameZh,
                   StopsData[] stops)
    {
        PlateNumb = plateNumb;
        if(plateNumb =="") PlateNumb = "查無車牌";

        RouteName = routeName;
        NextBusTime = nextBusTime;
        IsLastBus = isLastBus;
        StopStatus = stopstatus;
        BusStatus = busStatus;
        A2EventType = a2EventType;
        SubRoutes = subRoutes;
        Direction = direction;
        BusRouteType = busRouteType;
        DepartureStopNameZh = departureStopNameZh;
        DestinationStopNameZh = destinationStopNameZh;
        Stops = stops;
    }

    

}



