using back.Data;
using back.Data.Entities;
using File = back.Data.Entities.File;

namespace back.Services
{
    public class FileService
    {
        public FileService(IWebHostEnvironment hostingEnvironment, EfDbContex efDbContex)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.dbContex= efDbContex; 
        }

        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly EfDbContex dbContex;
        public async Task<List<File>> RangeFiles (List<IFormFile> formFiles, Guid ID)
        {
            if(formFiles.Count==0)
                return new List<File>();
           List<File> result = new List<File>();
            string uploads = Path.Combine(hostingEnvironment.WebRootPath, "MyResFiles");
            foreach (IFormFile file in formFiles)
            {
                if (file.Length > 0)
                {
                    string filePath = Path.Combine(uploads, file.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    result.Add(new File() { Name=file.FileName, Path=filePath, ToDoItemId=ID});
                    dbContex.Add(result.Last());
                    dbContex.SaveChanges();
                }
            }
            return result;
        }
    }
}
