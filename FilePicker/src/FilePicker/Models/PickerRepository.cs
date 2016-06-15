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
            Add(new FileItem { FileName = "test.txt"});
            Add(new FileItem { FileName = "Item2.jpeg" });
            Add(new FileItem { FileName = "Item3.mp3" });

            Add(new FileItem { FileName = "Item4.mp4" });
          
        }

        public IEnumerable<FileItem> GetAll()
        {
            return items;
        }

        public void Add(FileItem item)
        {
            item.ID = this.count;
            item.FileExtension = Path.GetExtension(item.FileName);
            item.FileName = Path.GetFileNameWithoutExtension(item.FileName);
            item.DateAdded = DateTime.Now;
            if (item.FileExtension.Equals(".mp4"))
            {
                item.FileType = "Video";
            }
            else if (item.FileExtension.Equals(".jpeg"))
            {
                item.FileType = "Image";
            }
            else if (item.FileExtension.Equals(".mp3"))
            {
                item.FileType = "Audio";
            }
            else
            {
                item.FileType = "Document";
            }

            items.Add(item);
            this.count++;
            items = items.OrderBy(t => t.DateAdded).ToList();

            //SAVE FILES TO DATABASE HERE
                //Will be done based off of url and/or users local directory
           // ProcessWrite(item.FileName);
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

        public async void ProcessWrite(string filename)
        {
            string filePath = @"C:\Projects\" + filename;
            string text = "Hello World\r\n";

            await WriteTextAsync(filePath, text);
        }

        private async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = System.Text.Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }
}
