using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TooDoo.Model
{
    public class ToDoItem
    {
        public int TodoItemId { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Display(Name="Details")]
        public string Details { get; set; }

        [Display(Name = "Due")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Completed")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy  H:mm}")]
        public DateTime? Completed { get; set; }

        [Display(Name = "Done")]
        public bool IsDone { get; set; }
    }
}