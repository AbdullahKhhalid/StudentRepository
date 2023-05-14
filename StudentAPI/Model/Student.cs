using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Model
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Year { get; set; }
        public int RollNumber { get; set; }
    }
}
