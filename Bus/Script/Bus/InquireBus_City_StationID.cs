using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

public class InquireBus_City_StationID : MonoBehaviour
{
    [SerializeField] Text JsonDamo;
    enum GetMode
    {
        EstimatedTimeOfArrival,
        RealTimeNearStop,
        Route
    }
    [SerializeField] GetMode getMode;
    [SerializeField] InputField cityInputField; //城市輸入
    [SerializeField] InputField stationIDInputField; //站位代碼輸入
    [SerializeField] Text message;      //顯示結果用訊息版型
    [SerializeField] Text amountText;      //顯示結果數量
    [SerializeField] Transform messageField;    //訊息版位置
    
    string apiData;    //API數據    
    bool IsOnSearch;    //防止連續搜尋

    
    [SerializeField] List<BusData> busDatas;      //資料結果存取陣列
    [SerializeField] List<Text> messages;
    void Start()
    {
        cityInputField.text = "Taichung";     //懶得每次都打
        stationIDInputField.text = "912";
    }
    public void GetBusApi()
    {        
        if(!IsOnSearch)
        {
            IsOnSearch = true;
            StartCoroutine(SearchBusInquireCoroutine());
        }        
    }
    IEnumerator SearchBusInquireCoroutine()
    {
        // Debug.Log("嘗試獲取資料來源\n"+ string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/{0}/City/{1}/PassThrough/Station/{2}?$top=30&$format=JSON", getMode,cityInputField.text, stationIDInputField.text));
        // RestClient.Get(string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/{0}/City/{1}/PassThrough/Station/{2}?$top=30&$format=JSON", getMode,cityInputField.text, stationIDInputField.text)
        //     ).Then(response => {      
        //             //saveData = JsonHelper.ArrayFromJson<StopSignData>(response.Text);
        //             apiData = response.Text.ToString();
        //         }
        //     );
        apiData = JsonDamo.text;    //先用Damo檔，以防每日50次上限用光
        Debug.Log("等待獲取API資料");
        yield return new WaitUntil(()=> apiData != null);  //等待資料或取後再進行打印
        Debug.Log("已獲取API資料");
        //yield return new WaitForSeconds(1);
        FetchContent();
        IsOnSearch = false;
    }

    /// <summary>
    /// 取得需要的內容
    /// </summary>
    void FetchContent()
    {
        Debug.Log("取得需要的內容");
        var saveData = JsonHelper.ArrayFromJson<BusData>(apiData);
        amountText.text = "搜尋結果數量: "+ saveData.Length;
        switch(getMode)
        {
            case GetMode.EstimatedTimeOfArrival:
                for (int i = 0; i < saveData.Length; i++)
                {
                    Text mess;
                    mess = Instantiate(message, messageField);
                    mess.text = string.Format("路線:{0}\n到站時間{1}\n是否為末班車:{2} 車輛狀況:{3}",
                        saveData[i].RouteName.Zh_tw, saveData[i].NextBusTime, saveData[i].IsLastBus, saveData[i].StopStatus);
                    busDatas.Add(saveData[i]);
                    messages.Add(mess);
                }   
            break;
            case GetMode.RealTimeNearStop:
                for (int i = 0; i < saveData.Length; i++)
                {
                    Text mess;
                    mess = Instantiate(message, messageField);
                    mess.text = string.Format("車牌號碼:{0} 路線:{1}\n行車狀況{2} 進站離站:{3}",
                        saveData[i].PlateNumb ,saveData[i].RouteName.Zh_tw, saveData[i].BusStatus, saveData[i].A2EventType);
                    busDatas.Add(saveData[i]);
                    messages.Add(mess);
                }   
            break;
            case GetMode.Route:
                for (int i = 0; i < saveData.Length; i++)
                {
                    Text mess;
                    mess = Instantiate(message, messageField);
                    mess.text = string.Format("SubRoutes:{0} 路線:{1} 往起始/終點站:{2}\n公車路線類別:{3}\n起始站:{4} 終點站{5}",
                        saveData[i].SubRoutes ,saveData[i].RouteName.Zh_tw, saveData[i].Direction, saveData[i].BusRouteType, saveData[i].DepartureStopNameZh, saveData[i].DestinationStopNameZh);
                    busDatas.Add(saveData[i]);
                    messages.Add(mess);
                }   
            break;
        }
             
    }
}

