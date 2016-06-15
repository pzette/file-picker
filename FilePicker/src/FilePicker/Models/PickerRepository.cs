using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FilePicker.Models
{
    public class PickerRepository : IPickerRepository
    {
        static List<FileItem> items =
              new List<FileItem>();
        private int count;
       
        public PickerRepository()
        {
            this.count = 0;
            Add(new FileItem { FileName = "Item1", DateAdded = Convert.ToDateTime("01/08/2008 14:50:50.42") });
            Add(new FileItem { FileName = "Item2" });
            Add(new FileItem { FileName = "Item3" });
            Add(new FileItem { FileName = "Item4" });
          
        }

        public IEnumerable<FileItem> GetAll()
        {
            return items;
        }

        public void Add(FileItem item)
        {
            item.ID = this.count;
            items.Add(item);
            this.count++;
            items = items.OrderBy(t => t.DateAdded).ToList();
        }

        public FileItem Find(int ID)
        {
            return items.Find(t => t.ID.Equals(ID));
        }

        public bool Remove(int ID)
        {
            FileItem item = items.Find(t => t.ID.Equals(ID));
            return items.Remove(item);
        }

        public void Update(FileItem item)
        {
            FileItem curr = items.Find(t => t.ID.Equals(item.ID));
            Delta<FileItem>(curr, item);
            items = items.OrderBy(t => t.DateAdded).ToList();
        }

        public void Delta<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }
    }
}
