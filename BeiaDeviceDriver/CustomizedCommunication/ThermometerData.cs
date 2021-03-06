﻿using System;
using Newtonsoft.Json;

namespace Safecare.BeiaDeviceDriver
{
    public class ThermometerData
    {
        [JsonProperty("Utc_date_time")]
        public DateTime Time { get; set; }

        [JsonProperty("Light_level")]
        public double LightLevel { get; set; }

        [JsonProperty("Temperature_in_degrees")]
        public double TemperatureInDegrees { get; set; }

        [JsonProperty("Humidity_level")]
        public double HumidityLevel { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    Formatting = Formatting.Indented,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
        }

        public static ThermometerData Deserialize(string text)
        {
            return JsonConvert.DeserializeObject<ThermometerData>(text);
        }

        public void SetDateTimeIfEmpty()
        {
            if (Time == default)
                Time = DateTime.UtcNow;
        }
    }
}
