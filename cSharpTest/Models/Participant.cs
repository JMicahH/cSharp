using System;
using System.ComponentModel.DataAnnotations;
using cSharpTest.Models;
using System.Collections.Generic;

namespace cSharpTest.Models
{
    public class Participant
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}