using System;
using System.ComponentModel.DataAnnotations;
using cSharpTest.Models;
using System.Collections.Generic;


namespace cSharpTest.Models
{
    public class Activity : BaseEntity
    {
        public int id { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int Duration { get; set; }
        public string DurationScope { get; set; }
        public string Desc { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
        public Activity()
        {
            Participants = new List<Participant>();
        }
    }


    public class ActivityViewModel : BaseEntity
    {

        [Required]
        [MinLength(2)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Time")]
        public DateTime Time { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [Required]
        public string DurationScope { get; set; }

        [Required]
        [MinLength(10)]
        [Display(Name = "Description")]
        public string Desc { get; set; }
    }
}
