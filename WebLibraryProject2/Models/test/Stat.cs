namespace WebLibraryProject2.Models.test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stat
    {
        public int Id { get; set; }

        public DateTime DateTaken { get; set; }

        public int Publication_Id { get; set; }

        public virtual Publication Publication { get; set; }
    }
}
