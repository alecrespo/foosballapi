using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ecovadis.DAL.Models
{
    [Table("Goal")]
    public class Goal : BaseEntity
    {
        public Goal() => Time = DateTime.UtcNow;
        public virtual TableSide Player { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Period")]
        public virtual int PeriodId { get; set; }
        public virtual Period Period { get; set; }
    }
}
