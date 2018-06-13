namespace WebLibraryProject2.Models.test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BookLocation")]
    public partial class BookLocation
    {
        public int Id { get; set; }

        public int Room { get; set; }

        [Required]
        [StringLength(70)]
        public string Place { get; set; }

        public bool IsTaken { get; set; }

        public int Publication_Id { get; set; }

        public int? Reader_Id { get; set; }

        public virtual Reader Reader { get; set; }

        public virtual Publication Publication { get; set; }
    }
}
