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
    public class GoalDto
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual TableSide Player { get; set; }
        public DateTime Time { get; set; }
    }
}
