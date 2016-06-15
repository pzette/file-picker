using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilePicker.Models
{
    public class FileItem
    {
        public String FileName { get; set; }
        public int ID { get; set; }
        public DateTime DateAdded { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public string Author { get; set; }
        public FileStream FileContent { get; set; }
    }
}
