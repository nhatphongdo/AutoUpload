using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace AutoUpload.Updater.Windows
{
    public class Program
    {
        public static readonly string CheckUpdateUrl = "http://autoupload.teeinsight.com/api/users/checkupdate";
        private static readonly string TemporaryDownloadFile = "temp.zip";

        public static void Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var content = httpClient.GetStringAsync(CheckUpdateUrl).Result;
                    var parts = content.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        if (File.Exists(TemporaryDownloadFile))
                        {
                            File.Delete(TemporaryDownloadFile);
                        }

                        // Download file
                        var inputStream = httpClient.GetStreamAsync(parts[1]).Result;
                        var buffer = new byte[4096];
                        using (var outputStream = File.OpenWrite(TemporaryDownloadFile))
                        {
                            int count;
                            while ((count = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                outputStream.Write(buffer, 0, count);
                            }
                            outputStream.Flush(true);
                        }

                        // Close processes
                        var processes = Process.GetProcesses();
                        foreach (var process in processes)
                        {
                            if (process.ProcessName.Equals("AutoUpload.Windows", StringComparison.CurrentCultureIgnoreCase))
                            {
                                process.Kill();
                                process.WaitForExit();
                            }
                        }

                        // Extract file
                        ExtractZipFile(TemporaryDownloadFile, Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

                        // Remove temporary file
                        File.Delete(TemporaryDownloadFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private static void ExtractZipFile(string archiveFilenameIn, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    var entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096]; // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outFolder, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }
    }
}