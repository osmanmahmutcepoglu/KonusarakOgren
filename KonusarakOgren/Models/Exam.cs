using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KonusarakOgren.Models
{
    public class Exam
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }
        [Display(Name = "Tarih")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        public IList<Question> Questions { get; set; }
    }
}
