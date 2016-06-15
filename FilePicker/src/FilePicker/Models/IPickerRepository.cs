using System.Collections.Generic;

namespace FilePicker.Models
{
    public interface IPickerRepository
    {
        void Add(FileItem item);
        IEnumerable<FileItem> GetAll();
        FileItem Find(int ID);
        bool Remove(int ID);
        void Update(FileItem item);
    }
}
