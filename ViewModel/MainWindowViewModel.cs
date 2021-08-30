using MVVMDemo.Helper;
using MVVMDemo.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MVVMDemo.ViewModel
{
    public class MainWindowViewModel : NotificationObject
    {
        private readonly WeatherHelper weatherHelper = new WeatherHelper();

        private WeatherInfo _weatherInfo;

        public MainWindowViewModel()
        {
            GetWeatherAsync();
        }

        private async void GetWeatherAsync()
        {
            string res = await weatherHelper.GetWeather();

            JObject jo = (JObject)JsonConvert.DeserializeObject(res);
            if (jo["status"].ToString() == "200")
            {
                Weather = CreateWeatherInfo(jo["msg"]["city"]["@attributes"]);
            }
            else if (jo["status"].ToString() == "500")
            {
                //无法获取天气信息
                System.Console.WriteLine("无法获取天气信息");
            }
        }

        public WeatherInfo Weather
        {
            get { return _weatherInfo; }
            set
            {
                _weatherInfo = value;
                RaiseProertyChanged("Weather");
            }
        }


        private WeatherInfo CreateWeatherInfo(JToken jToken)
        {
            WeatherInfo weatherInfo = new WeatherInfo();

            weatherInfo.CityName = jToken["cityname"].ToString();
            weatherInfo.CenterName = jToken["centername"].ToString();
            weatherInfo.State1 = jToken["state1"].ToString();
            weatherInfo.State2 = jToken["state2"].ToString();
            weatherInfo.StateDetailed = $"气候 {jToken["stateDetailed"]}";
            weatherInfo.Tem1 = $"最低温度 {jToken["tem1"]}℃";
            weatherInfo.Tem2 = $"最高温度 {jToken["tem2"]}℃";
            weatherInfo.TemNow = $"当前温度 {jToken["temNow"]}℃";
            weatherInfo.WindState = jToken["windState"].ToString();
            weatherInfo.WindDir = $"风向 {jToken["windDir"]}";
            weatherInfo.WindPower = $"风力 {jToken["windPower"]}";
            weatherInfo.Humidity = $"湿度 {jToken["humidity"]}";
            weatherInfo.Time = $"时间 {jToken["time"]}";
            weatherInfo.Url = $"地区代码 {jToken["url"]}";

            return weatherInfo;
        }
    }
}
