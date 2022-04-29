using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Persistence
{
    public static class FilterExtensions
    {
        public static IEnumerable<Pet> FilterBySize(this IEnumerable<Pet> pets, string[] sizes)
        {
            if(sizes?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => sizes.Contains(x.Size.ToString()));
            }

            return pets;
        }
        
        public static IEnumerable<Pet> FilterBySex(this IEnumerable<Pet> pets, string[] sexes)
        {
            if(sexes?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => sexes.Contains(x.Sex.ToString()));
            }

            return pets;
        }
        public static IEnumerable<Pet> FilterByCity(this IEnumerable<Pet> pets, string[] cities)
        {
            if (cities?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => cities.Contains(x.Shelter.City.ToString()));
            }

            return pets;
        }

        public static IEnumerable<Pet> FilterByType(this IEnumerable<Pet> pets, string[] types)
        {
            if(types?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => types.Contains(x.Type.ToString()));
            }

            return pets;
        }

        public static IEnumerable<Pet> FilterByColor(this IEnumerable<Pet> pets, string[] colors)
        {
            if (colors?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => colors.Contains(x.Color.ToString()));
            }

            return pets;
        }
        public static IEnumerable<Pet> FilterByAge(this IEnumerable<Pet> pets, int min, int max)
        {
            if (max>min)
            {
                pets = pets.Where(x => min<=x.Years && x.Years<max);
            }

            return pets;
        }

        public static IEnumerable<Shelter> FilterByCity(this IEnumerable<Shelter> shelters, string[] cities)
        {
            if(cities?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                shelters = shelters.Where(x => cities.Contains(x.City));
            }

            return shelters;
        }

        public static IEnumerable<Reservation> FilterByUser(this IEnumerable<Reservation> reservations, Guid userId)
        {
            if (Guid.Empty != userId)
            {
                reservations = reservations.Where(x => x.UserId == userId);
            }
            return reservations;
        }

        public static IEnumerable<Reservation> FilterByShelter(this IEnumerable<Reservation> reservations, Guid shelterId)
        {
            if (Guid.Empty != shelterId)
            {
                reservations = reservations.Where(x => x.ShelterId==shelterId);
            }
            return reservations;
        }

        public static IEnumerable<Reservation> FilterByDate(this IEnumerable<Reservation> reservations, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (startDate < endDate)
            {
                reservations = reservations.Where(x => x.StartTime>startDate && x.EndTime<endDate);
            }
            return reservations;
        }
    }
}
