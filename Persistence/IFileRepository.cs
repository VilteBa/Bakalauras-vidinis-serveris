using backend.Controllers;
using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.Persistence
{
    public interface IFileRepository
    {
        IEnumerable<File> GetPetPhotos(Guid petId);
        File GetShelterPhoto(Guid shelterId);
        void CreateFile(File file);
        void DeleteFile(Guid fileId);
    }
}