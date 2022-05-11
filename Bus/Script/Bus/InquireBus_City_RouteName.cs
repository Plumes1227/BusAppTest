using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;

public class InquireBus_City_RouteName : MonoBehaviour
{
    [SerializeField] Text JsonDamo;
    [SerializeField] InputField cityInputField; //城市輸入
    [SerializeField] InputField routeNameInputField; //路線名稱輸入
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
        routeNameInputField.text = "307";
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
        // Debug.Log("嘗試獲取資料來源\n"+ string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/DisplayStopOfRoute/City/{0}/{1}?$top=30&$format=JSON", cityInputField.text, routeNameInputField.text));
        // RestClient.Get(string.Format("https://ptx.transportdata.tw/MOTC/v2/Bus/DisplayStopOfRoute/City/{0}/{1}?$top=30&$format=JSON", cityInputField.text, routeNameInputField.text)
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

    string StopsMess;
    /// <summary>
    /// 取得需要的內容
    /// </summary>
    void FetchContent()
    {
        Debug.Log("取得需要的內容");
        var saveData = JsonHelper.ArrayFromJson<BusData>(apiData);
        amountText.text = "搜尋結果數量: "+ saveData.Length;
        for (int i = 0; i < saveData.Length; i++)
        {
            StopsMess = "";
            Text mess;
            mess = Instantiate(message, messageField);            
            for (int j = 0; j < saveData[i].Stops.Length; j++)
            {
                StopsMess += saveData[i].Stops[j].StopName.Zh_tw + ">";
            }
            mess.text = string.Format("去返程:{0} 所有經過站牌:\n{1}", saveData[i].Direction, StopsMess);
            busDatas.Add(saveData[i]);            
            messages.Add(mess);
        }
    }
}
