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

        // Mass in kg of the CelestialObject
        public double Mass { get; set; }

        // Mean Radius in km of the CelestialObject
        public double Radius { get; set; }

        // Object being orbited (note this implementation will not work in binary systems)
        public CelestialObject OrbitedObject { get; set; }

        // Objects that orbit this object
        [NotMapped]
        public List<CelestialObject> Satellites { get; set; }

        // Time it takes to complete one full orbit in days
        public TimeSpan OrbitalPeriod { get; set; }

        // Average speed in km/s of orbiting object relative to what it's orbiting
        public decimal AverageOrbitalSpeed { get; set; }

        // Furthest point in orbit from the object orbited in km
        public double Aphelion { get; set; }

        // Closest point in orbit to the object orbited in km
        public double Perihelion { get; set; }

        // Tilt of the orbiting object relative to the orbited object's rotation
        public decimal Inclination { get; set; }

        // Amount an orbit deviates from a perfect circle 
        public decimal Eccentricity { get; set; }
    }
}
