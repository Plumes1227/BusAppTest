/// <summary>
/// 站牌結構,目前只獲取:城市,站位代碼
/// </summary>
[System.Serializable]
public class StopSignData
{
    public string StationID;
    public string City;

    public StopSignData(string id, string city)
    {
        StationID = id;
        City = city;        
        if(City == "") City = "此為公路/國道客運路線";
    }
}
    //API文本所有範例KEY
    /*
    public string StopUID
    public string StopID
    public string AuthorityID
    public StopName stopName
    public class StopName
    {
        public string Zh_tw
        public string En
    }

    public StopPosition stopPosition
    public class StopPosition
    {
        public float PositionLon
        public float PositionLat
        public string GeoHash
    }

    public string StopAddress
    public string Bearing     
    public string StationID
    public string StationGroupID
    public string StopDescription
    public string City
    public string CityCode
    public string LocationCityCode
    public string UpdateTime
    public int VersionID
    */
