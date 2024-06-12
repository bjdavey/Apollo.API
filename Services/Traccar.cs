using API.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Apollo.API.Services
{
    public class Device
    {
        public int? id { set; get; }
        public string? uniqueId { set; get; }
        public string? status { set; get; }
        public DateTime? lastUpdate { set; get; }
        public decimal? latitude { set; get; }
        public decimal? longitude { set; get; }
        public decimal? altitude { set; get; }
    }

    public class Traccar
    {
        private static string BaseUrl = "http://185.222.241.148:8082/api";
        private static string BasicAuth = "amJqZGF2ZXlAaG90bWFpbC5jb206MDAwMA==";

        private class DeviceResponse
        {
            public int? id { set; get; }
            public string? uniqueId { set; get; }
            public string? status { set; get; }
            public DateTime? lastUpdate { set; get; }
            public int? positionId { set; get; }
        }

        private class PositionResponse
        {
            public int? id { set; get; }
            public int? deviceId { set; get; }
            public decimal? latitude { set; get; }
            public decimal? longitude { set; get; }
            public decimal? altitude { set; get; }
        }


        public static async Task<int?> CreateDevice(string name, string uniqueId)
        {
            var auth = new AuthenticationHeaderValue("Basic", BasicAuth);
            var content = Utilities.JsonContent(new
            {
                name = name,
                uniqueId = uniqueId
            });
            var response = await Utilities.PostRequest(BaseUrl, $"/devices", content, auth);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<DeviceResponse>(jsonString);
                return res?.id;
            }
            return null;
        }

        public static async Task<Device?> FetchDevice(string uniqueId)
        {
            var auth = new AuthenticationHeaderValue("Basic", BasicAuth);
            var getDevices = await Utilities.GetRequest(BaseUrl, $"/devices?uniqueId={uniqueId}", auth);
            if (getDevices.StatusCode == HttpStatusCode.OK)
            {
                var getDevicesJsonString = await getDevices.Content.ReadAsStringAsync();
                var devices = JsonConvert.DeserializeObject<DeviceResponse[]>(getDevicesJsonString);
                var device = devices?[0];
                if (device != null)
                {
                    var response = new Device()
                    {
                        id = device?.id,
                        uniqueId = device?.uniqueId,
                        lastUpdate = device?.lastUpdate,
                        status = device?.status
                    };
                    var positionId = device?.positionId;
                    if (positionId != null)
                    {
                        var getPositions = await Utilities.GetRequest(BaseUrl, $"/positions?id={positionId}", auth);
                        if (getPositions.StatusCode == HttpStatusCode.OK)
                        {
                            var getPositionsJsonString = await getPositions.Content.ReadAsStringAsync();
                            var positions = JsonConvert.DeserializeObject<PositionResponse[]>(getPositionsJsonString);
                            response.longitude = positions?[0]?.longitude;
                            response.latitude = positions?[0]?.latitude;
                            response.altitude = positions?[0]?.altitude;
                        }
                    }
                }
            }
            return null;
        }

    }
}
