using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecovadis.DAL.Models
{
    [Table("Period")]
    public class Period: BaseEntity
    {
        public Period()
        {
            StartTime = DateTime.UtcNow;
            Goals = new List<Goal>();
        }
        public bool IsFinished { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [ForeignKey("Match")]
        public  int MatchId { get; set; }
        public virtual Match Match { get; set; }
        public virtual TableSide Winner { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }

    }
}
