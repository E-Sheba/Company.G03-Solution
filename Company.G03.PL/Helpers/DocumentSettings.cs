using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Company.G03.PL.Helpers
{
    public static class DocumentSettings
    {
        //1. Upload

        public static string Upload(IFormFile file,string folderName)
        {
            //1.Get Folder Location
            //Get Current Directory gets the location fo the project 
            //then we add the path to where the containing folder exist and the folder name will be either audio - images or videos

            //string folderPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\files\\{folderName}";

            //Combine method
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\files\\{folderName}");


            //Get file name // Uniqueeeeeeeeeeee   ====> the image name , the video name or the audio name 
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            //Get File Path   ===> Folder Path + File Name
            string filePath = Path.Combine(folderPath, fileName);

            //CopyTo() takes parameter of type stream

            //4. File Stream ==> Data per second
            //FileMode.Create takes the path provided and deals with it  .... in other words it opens a connection with an unmanaged resource [File in this case]
            //So I need to close the connection after the FileMode.Create Opened it ....... so I must use [using <= Before the fileStream where the connection opened in order to close it ]
           using var fileStream = new FileStream(filePath,FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }

        //2.Delete
        public static void Delete(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\files\\{folderName}",fileName);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }



        //2.Delete
    }
}
