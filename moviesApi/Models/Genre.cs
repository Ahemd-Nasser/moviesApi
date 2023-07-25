using System.ComponentModel.DataAnnotations.Schema;

namespace moviesApi.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { set; get; }
        [MaxLength(50)]
        public string Name { set; get; }

    }
}
