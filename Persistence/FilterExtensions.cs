using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        //gal kitaip?
        public static IEnumerable<Pet> FilterBySex(this IEnumerable<Pet> pets, string[] sexes)
        {
            if(sexes?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => sexes.Contains(x.Sex.ToString()));
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

        public static IEnumerable<Shelter> FilterByCity(this IEnumerable<Shelter> shelters, string[] cities)
        {
            if(cities?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                shelters = shelters.Where(x => cities.Contains(x.City));
            }

            return shelters;
        }
    }
}
