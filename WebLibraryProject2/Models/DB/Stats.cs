using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{
    [Table("Stats")]
    public partial class Stats
    {
        public int Id { get; set; }

        public DateTime DateTaken { get; set; }

        public int Publications { get; set; }

        public virtual Publication Publication { get; set; }
    }
}
