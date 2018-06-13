using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{
    [Table("BookLocation")]
    public partial class BookLocation
    {
        public int Id { get; set; }

        public int Room { get; set; }

        [Required]
        [StringLength(70)]
        public string Place { get; set; }

        public bool IsTaken { get; set; }

        public int Publications { get; set; }

        public int? Readers { get; set; }

        public virtual Reader Reader { get; set; }

        public virtual Publication Publication { get; set; }


        public BookLocation()
        {
        }
        public BookLocation(int Room, string Place) : this()
        {
            this.Room = Room;
            this.Place = Place;
        }
        public BookLocation(Reader Reader)
        {
            this.Reader = Reader;
        }

        public BookLocation Clone()
        {
            return new BookLocation(Room, Place) { Reader = Reader, IsTaken = IsTaken, Publication = Publication };
        }

        public override string ToString() => IsTaken ? $"{Reader}" : $"{Room}, {Place}";
    }
}
