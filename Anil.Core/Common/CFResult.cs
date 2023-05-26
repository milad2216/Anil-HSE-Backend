
using Newtonsoft.Json;
using static Anil.Core.Infrastructure.Enums.Types;

namespace Anil.Core.Common
{
    public class CFResult
    {
        [JsonProperty(PropertyName = "status")]
        public OperationResultType Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public object? Errors { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string? ID { get; set; }
        [JsonProperty(PropertyName = "longid")]
        public long LongID { get; set; }
        [JsonProperty(PropertyName = "extra")]
        public object? Extra { get; set; }
    }

    public class CFResult<TViewModel>
    {
        [JsonProperty(PropertyName = "status")]
        public OperationResultType Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string? Message { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public object? Errors { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string? ID { get; set; }
        [JsonProperty(PropertyName = "longid")]
        public long LongID { get; set; }

        [JsonProperty(PropertyName = "extra")]
        public TViewModel? Extra { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<TViewModel>? Data { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public int? RecordsTotal { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public int? RecordsFiltered { get; set; }
    }
}
