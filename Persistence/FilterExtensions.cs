using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Persistence
{
    public static class FilterExtensions
    {
        public static IQueryable<Pet> FilterBySize(this IQueryable<Pet> pets, string[] sizes)
        {
            if(sizes?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => sizes.Contains(x.Size.ToString()));
            }

            return pets;
        }
        //gal kitaip?
        public static IQueryable<Pet> FilterBySex(this IQueryable<Pet> pets, string[] sexes)
        {
            if(sexes?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => sexes.Contains(x.Sex.ToString()));
            }

            return pets;
        }

        public static IQueryable<Pet> FilterByType(this IQueryable<Pet> pets, string[] types)
        {
            if(types?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                pets = pets.Where(x => types.Contains(x.Type.ToString()));
            }

            return pets;
        }

        public static IQueryable<Shelter> FilterByCity(this IQueryable<Shelter> shelters, string[] cities)
        {
            if(cities?.Any(x => !string.IsNullOrEmpty(x)) == true)
            {
                shelters = shelters.Where(x => cities.Contains(x.City));
            }

            return shelters;
        }
    }
}
