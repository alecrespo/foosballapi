using Ecovadis.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Ecovadis.DL.Models
{
    [ModelBinder]
    public class PeriodDto
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual TableSide Winner { get; set; }
        public bool IsFinished { get; set; }
        public virtual ICollection<GoalDto> Goals { get; set; }
    }
}
