using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Persistence
{
    public class FileRepository : IFileRepository
    {
        private readonly DBContext _dbContext;

        public FileRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void CreateFile(File file)
        {
            _dbContext.Files.Add(file);
            _dbContext.SaveChanges();
        }

        public void DeleteFile(Guid id)
        {
            var file = _dbContext.Files.SingleOrDefault(file => file.FileId == id);
            _dbContext.Files.Remove(file ?? throw new InvalidOperationException());
            _dbContext.SaveChanges();
        }

        public IEnumerable<File> GetPetPhotos(Guid petId)
        {
            return _dbContext.Files.Where(file => file.PetId == petId);
        }

        public File GetShelterPhoto(Guid shelterId)
        {
            return _dbContext.Files.Where(file => file.ShelterId == shelterId).FirstOrDefault();
        }
    }
}