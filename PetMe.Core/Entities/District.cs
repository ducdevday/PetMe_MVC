using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetMe.Data.Entities
{
    public class District
    {
        [JsonPropertyName("district_id")]
        public string DistrictId { get; set; }
        [JsonPropertyName("district_name")]
        public string DistrictName { get; set; }
        [JsonPropertyName("district_type")]
        public string DistrictType { get; set; }
        [JsonPropertyName("province_id")]
        public string ProvinceId { get; set; }
    }
}
