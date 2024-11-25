using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebStatusResult
    {
        [JsonPropertyName("hostName")]
        public string HostName { get; set; }

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonPropertyName("operationMode")]
        public string OperationMode { get; set; }

        [JsonPropertyName("haStatus")]
        public string HaStatus { get; set; }

        [JsonPropertyName("systemTime")]
        public string SystemTime { get; set; }

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("up_days")]
        public string UpDays { get; set; }

        [JsonPropertyName("up_hrs")]
        public string UpHrs { get; set; }

        [JsonPropertyName("up_mins")]
        public string UpMins { get; set; }

        [JsonPropertyName("firmware_partition")]
        public int FirmwarePartition { get; set; }

        [JsonPropertyName("administrativeDomain")]
        public string AdministrativeDomain { get; set; }

        [JsonPropertyName("vmLicense")]
        public string VmLicense { get; set; }

        [JsonPropertyName("registration")]
        public FortiWebRegistration Registration { get; set; }

        [JsonPropertyName("readonly")]
        public bool Readonly { get; set; }

        [JsonPropertyName("bufferSizeMax")]
        public int BufferSizeMax { get; set; }

        [JsonPropertyName("fileUploadLimitMax")]
        public int FileUploadLimitMax { get; set; }
    }
}
