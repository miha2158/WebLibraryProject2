namespace WebLibraryProject2.Models.test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Author")]
    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            Publications = new HashSet<Publication>();
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

        public byte WriterType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Publication> Publications { get; set; }
    }
}
