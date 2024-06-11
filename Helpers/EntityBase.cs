using Apollo.API.Models.DB;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Helpers
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
        //public Guid Uid { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public bool? IsDeleted { get; set; }

        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [ForeignKey(nameof(CreatedBy))]
        [ReadOnly(true)]
        public User? CreatedByUser { get; set; }
    }

    //public class CreatedByUser
    //{
    //    [Key]
    //    public int Id { get; set; }
    //    public string UserName { get; set; }
    //    public string Name { get; set; }
    //}

}
