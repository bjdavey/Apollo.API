using API.Helpers;
using Newtonsoft.Json;
using System.Net;

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
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/devices");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Basic amJqZGF2ZXlAaG90bWFpbC5jb206MDAwMA==");
            var content = Utilities.JsonContent(new
            {
                name = name,
                uniqueId = uniqueId
            });
            request.Content = content;
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<DeviceResponse>(jsonString);
                return res?.id;
            }

            //var auth = new AuthenticationHeaderValue("Basic", BasicAuth);
            //var content = Utilities.JsonContent(new
            //{
            //    name = name,
            //    uniqueId = uniqueId
            //});
            //var response = await Utilities.PostRequest(BaseUrl, $"/devices", content, auth);
            //if (response.StatusCode == HttpStatusCode.OK)
            //{
            //    var jsonString = await response.Content.ReadAsStringAsync();
            //    var res = JsonConvert.DeserializeObject<DeviceResponse>(jsonString);
            //    return res?.id;
            //}
            return null;
        }

        public static async Task<Device?> FetchDevice(string uniqueId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/devices?uniqueId={uniqueId}");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Basic amJqZGF2ZXlAaG90bWFpbC5jb206MDAwMA==");
            var getDevices = await client.SendAsync(request);

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
                    if (positionId != null && positionId != 0)
                    {

                        var client2 = new HttpClient();
                        var request2 = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/positions?id={positionId}");
                        request2.Headers.Add("Accept", "application/json");
                        request2.Headers.Add("Authorization", "Basic amJqZGF2ZXlAaG90bWFpbC5jb206MDAwMA==");
                        var getPositions = await client2.SendAsync(request2);
                        if (getPositions.StatusCode == HttpStatusCode.OK)
                        {
                            var getPositionsJsonString = await getPositions.Content.ReadAsStringAsync();
                            var positions = JsonConvert.DeserializeObject<PositionResponse[]>(getPositionsJsonString);
                            response.longitude = positions?[0]?.longitude;
                            response.latitude = positions?[0]?.latitude;
                            response.altitude = positions?[0]?.altitude;
                        }
                    }
                    return response;
                }
            }
            return null;
        }

    }
}
