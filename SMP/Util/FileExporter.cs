using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SMP.Util
{
    public static class FileExporter
    {
        public static void Save(IJSRuntime jsRuntime, byte[] byteData, string mimeType, string fileName)
        {
            if (byteData == null)
            {
                jsRuntime.InvokeVoidAsync("alert", "The byte array provided for Exporting was Null.");
            }
            else
            {
                jsRuntime.InvokeVoidAsync("saveFile", Convert.ToBase64String(byteData), mimeType, fileName);
            }
        }

        public static async Task<UploadedFileDetails> Upload(IJSRuntime jsRuntime, string inputID)
        {
            UploadedFileDetails fileDetails = new UploadedFileDetails();

            JsonElement fileAsJson = await jsRuntime.InvokeAsync<JsonElement>("getUploadedFile", inputID);
            string fileAsString = fileAsJson.ToString();

            if (!string.IsNullOrEmpty(fileAsString))
            {
                Dictionary<string, string> uploadData = JsonSerializer.Deserialize<Dictionary<string, string>>(fileAsString);
                fileDetails.Name = uploadData["fileName"];
                fileDetails.Data = Convert.FromBase64String(uploadData["fileData"]);
            }

            return fileDetails;
        }
    }

    public class UploadedFileDetails
    {
        public string Name { get; set; } = string.Empty;

        public byte[] Data { get; set; }
    }
}
