using Newtonsoft.Json;
using System.Collections.Generic;

namespace QK.QAPP.Entity.ExtendEntity
{
    public class ContractResult
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("content")]
        public List<Contract> Content { get; set; }
    }

    public class Contract
    {
        [JsonProperty("serialno")]
        public string Serialno { get; set; }

        [JsonProperty("idnum")]
        public string Idnum { get; set; }

        [JsonProperty("pactno")]
        public string Pactno { get; set; }

        [JsonProperty("pactamt")]
        public double Pactamt { get; set; }

        [JsonProperty("principle")]
        public double Principle { get; set; }

        [JsonProperty("interest")]
        public double Interest { get; set; }

        [JsonProperty("returnsum")]
        public string Returnsum { get; set; }

        [JsonProperty("bromanfee")]
        public double Bromanfee { get; set; }

        [JsonProperty("termnum")]
        public string Termnum { get; set; }

        [JsonProperty("opndate")]
        public string Opndate { get; set; }

        [JsonProperty("enddate")]
        public string Enddate { get; set; }

        [JsonProperty("returntype")]
        public string Returntype { get; set; }

        [JsonProperty("filler")]
        public string Filler { get; set; }
    }
}
