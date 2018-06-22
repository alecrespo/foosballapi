using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecovadis.DAL.Models
{
    [Flags]
    public enum TableSide
    {
        None = 0,
        Blue = 1,
        Red = 2
    }
    [Table("Match")]
    public class Match : BaseEntity
    {
        public Match(){
            StartTime = DateTime.UtcNow;
            Periods = new List<Period>();
        }
        public bool WasOneSided { get; set; }
        public bool IsFinished { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public virtual TableSide Winner { get; set; }
        public virtual ICollection<Period> Periods { get; set; }
    }
}
