using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FilePicker.Models;


namespace FilePicker.Controllers
{
    [Route("api/[controller]")]
    public class PickerController : Controller
    {
        public IPickerRepository FileItems { get; set; }


        public PickerController(IPickerRepository fileItems)
        {
            FileItems = fileItems;

            
        }


        [HttpGet]
        public IEnumerable<FileItem> Get(string name = "", 
                                            string author = "",
                                            string sort = "", 
                                            bool reverse = false)
        {
            sort = sort.ToLower();
            var temp = FileItems.GetAll().AsEnumerable();
            if(name != "")
            {
                temp = temp.Where(t => t.FileName.ToLower().Contains(name.ToLower()));
            }
            if (sort.Equals("filetype"))
                temp = temp.OrderBy(t => t.FileName);

            if (reverse)
                temp = temp.Reverse();

            return temp;

        }



        //public async Task Post(ICollection<IFormFile> files)
        //{
        //    var uploads = Path.Combine(hostingEnv.WebRootPath, "uploads");
        //    foreach (var file in files)
        //    {
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            await file.CopyToAsync(Stream());
        //        }
        //    }
        //}
        //    }
        //    //var filePath = fileName + "";
        //    //FileStream f = new FileStream(filePath, FileMode.Open);

        //    //byte[] buffer = { };
        //    //int x = await f.ReadAsync(buffer, 0, 0);
        //    //f.

        //    //await file.CopyToAsync(stream);

        //    //file.Dispose();
        //}

        [HttpGet("{id}", Name = "GetFile")]
        public IActionResult GetById(int id)
        {
            var item = FileItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        [ActionName("Complex")]
        public IActionResult Post([FromBody]FileItem item) // removed the FromBody
        {
            if (item == null)
            {
                return BadRequest();
            }

            FileItems.Add(item);
            return CreatedAtRoute("GetFile", new { controller = "Picker", id = item.ID }, item);
        }

   

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] FileItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            var itm = FileItems.Find(id);
            if (itm == null)
            {
                return NotFound();
            }

            item.ID = id;

            FileItems.Update(item);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            FileItems.Remove(id);
        }
    }
}
