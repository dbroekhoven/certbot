using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("can_view")]
        public int CanView { get; set; }

        [JsonPropertyName("q_ref")]
        public int QRef { get; set; }

        [JsonPropertyName("can_clone")]
        public int CanClone { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("type_val")]
        public string TypeVal { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("protocol_val")]
        public string ProtocolVal { get; set; }

        [JsonPropertyName("server-balance")]
        public string ServerBalance { get; set; }

        [JsonPropertyName("server-balance_val")]
        public string ServerBalanceVal { get; set; }

        [JsonPropertyName("health")]
        public string Health { get; set; }

        [JsonPropertyName("health_val")]
        public string HealthVal { get; set; }

        [JsonPropertyName("lb-algo")]
        public string LbAlgo { get; set; }

        [JsonPropertyName("lb-algo_val")]
        public string LbAlgoVal { get; set; }

        [JsonPropertyName("persistence")]
        public string Persistence { get; set; }

        [JsonPropertyName("persistence_val")]
        public string PersistenceVal { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("flag")]
        public int Flag { get; set; }

        [JsonPropertyName("server-pool-id")]
        public string ServerPoolId { get; set; }

        [JsonPropertyName("sub_table_id")]
        public int SubTableId { get; set; }

        [JsonPropertyName("sub_table_action")]
        public string SubTableAction { get; set; }

        [JsonPropertyName("sub_table_action_val")]
        public string SubTableActionVal { get; set; }

        [JsonPropertyName("used")]
        public int Used { get; set; }

        [JsonPropertyName("http-reuse")]
        public string HttpReuse { get; set; }

        [JsonPropertyName("http-reuse_val")]
        public string HttpReuseVal { get; set; }

        [JsonPropertyName("reuse-conn-total-time")]
        public int ReuseConnTotalTime { get; set; }

        [JsonPropertyName("reuse-conn-idle-time")]
        public int ReuseConnIdleTime { get; set; }

        [JsonPropertyName("reuse-conn-max-request")]
        public int ReuseConnMaxRequest { get; set; }

        [JsonPropertyName("reuse-conn-max-count")]
        public int ReuseConnMaxCount { get; set; }

        [JsonPropertyName("adfs-server-name")]
        public string AdfsServerName { get; set; }

        [JsonPropertyName("sz_pserver-list")]
        public int SzPserverList { get; set; }
    }
}
