using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KonusarakOgren.Models
{
    public class ExamTitle
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Href { get; set; }
    }
}
