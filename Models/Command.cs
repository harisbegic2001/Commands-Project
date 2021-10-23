using System.ComponentModel.DataAnnotations;

namespace Commander.Models{

    public class Command{ //Obični POJO? --> Odnosno klasa sa atributima
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        
        public string Line { get; set; }
        [Required]
        public string Platform { get; set; }

    }

}