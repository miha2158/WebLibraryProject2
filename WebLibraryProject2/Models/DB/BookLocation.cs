using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace WebLibraryProject2.Models
{
    public partial class BookLocation
    {
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
