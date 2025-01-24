using BoilerPlate.Models;

namespace BoilerPlate.Services
{
    public class TemplatesServices
    {
        Response _objResponse;
        public TemplatesServices() {
            _objResponse = new Response();
        }
        
        public Response GetTemplate(string fileName)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string path = Path.Combine(currentDirectory, $"Files/{fileName}");
            using (StreamReader streamReader = new StreamReader(path))
            {
                // Read the entire file content
                string fileContent = streamReader.ReadToEnd();

                // Print the file content
                Console.WriteLine(fileContent);
                _objResponse.Data = fileContent;
                _objResponse.IsError = false;
            }
            return _objResponse;
        }
    }
}
