using System;
using System.Collections.Generic;

namespace Capstone4TaskListRevisited.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public bool Complete { get; set; }
        public string EmployeeId { get; set; }

        public virtual AspNetUsers Employee { get; set; }

    }
}
