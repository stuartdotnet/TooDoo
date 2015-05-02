using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TooDoo.Model;

namespace TooDoo.Models
{
    public class EditToDoViewModel
    {
        public ToDoItem ToDoItem { get; set; }
        public bool WasDone { get; set; }
    }
}