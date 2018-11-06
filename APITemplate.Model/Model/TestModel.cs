using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APITemplate.Model.Model
{
    public class TestModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TestText { get; set; }
    }
}
