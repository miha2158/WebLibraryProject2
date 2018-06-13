namespace WebLibraryProject2.Models.test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reader")]
    public partial class Reader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reader()
        {
            BookLocations = new HashSet<BookLocation>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string First { get; set; }

        [Required]
        [StringLength(15)]
        public string Last { get; set; }

        [Required]
        [StringLength(15)]
        public string Patronimic { get; set; }

        public byte AccessLevel { get; set; }

        [Required]
        [StringLength(9)]
        public string Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookLocation> BookLocations { get; set; }
    }
}
