using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarChart.Models
{
    public class CelestialObject
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Object being orbited (note: this implementation will not work in binary systems)
        public CelestialObject OrbitedObject { get; set; }

        // Objects that orbit this object
        [NotMapped]
        public List<CelestialObject> Satellites { get; set; }

        // Time it takes to complete one full orbit in days
        public TimeSpan OrbitalPeriod { get; set; }
    }
}
