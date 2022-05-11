using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;


public class SearchBusStopSign : MonoBehaviour
{
    [SerializeField] Text JsonDamo;
    [SerializeField] InputField lat;    //經度 22~25
    [SerializeField] InputField lon;    //緯度 120~122
    [SerializeField] InputField searchRange;    //搜索範圍
    [SerializeField] Text message;      //顯示結果用訊息版型
    [SerializeField] Text amountText;      //顯示結果數量
    [SerializeField] Transform messageField;    //訊息版位置

    [SerializeField] int topAmount = 10;     //最前幾搜尋結果
    
    string apiData;    //API數據    
    bool IsOnSearch;    //防止連續搜尋

    [SerializeField] List<StopSignData> stopSignDatas;      //資料結果存取陣列
    [SerializeField] List<Text> messages;

    void Start()
    {
        lat.text = "25.047655";     //懶得每次都打
        lon.text = "121.517035";
    }

    //開始搜尋
    public void GetBusApi()
    {        
        if(!IsOnSearch)
        {
            IsOnSearch = true;
            StartCoroutine(SearchBusInquireCoroutine());
        }
        
    }
    //抓取API資料
    IEnumerator SearchBusInquireCoroutine()
    {
        // Debug.Log("嘗試獲取資料來源\n"+ string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/Stop/NearBy?$top=30&$spatialFilter=nearby({0},{1},{2})&$format=JSON", lat.text, lon.text, searchRange.text));
        // RestClient.Get(string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/Stop/NearBy?$top={0}&$spatialFilter=nearby({1},{2},{3})&$format=JSON", topAmount, lat.text, lon.text, searchRange.text)
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
        Debug.Log("取出城市及站位代碼");
        var saveData = JsonHelper.ArrayFromJson<StopSignData>(apiData);
        amountText.text = "搜尋結果數量: "+ saveData.Length;
        for (int i = 0; i < saveData.Length; i++)
        {
            Text mess;
            mess = Instantiate(message, messageField);
            mess.text = "城市: " + saveData[i].City + " 站位代碼: " + saveData[i].StationID;
            stopSignDatas.Add(saveData[i]);
            messages.Add(mess);
        }        
    } 
}

