using System;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.Compression;

namespace PresenterPlanner.Lib
{
	public class Zip
	{   
		public static void Compress(FileInfo fileToCompress)
		{
			using (FileStream originalFileStream = fileToCompress.OpenRead())
			{
				if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
				{
					if (File.Exists(fileToCompress.FullName + ".gz")) {
						File.Delete(fileToCompress.FullName + ".gz");
						Console.WriteLine ("File '{0}' was deleted", fileToCompress.FullName + ".gz");
						Thread.Sleep (10000);
					}

					using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
					{
						using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
						{
							originalFileStream.CopyTo(compressionStream);
							Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
								fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
						}
					}
				}
			}
			// Open the stream and read it back. 
			string fileText = File.ReadAllText (fileToCompress.FullName + ".gz");
			Console.WriteLine ("Readed {0} chars", fileText.Length.ToString());
			Thread.Sleep (10000);
			string hash = UploadFiles.GetMd5Hash (fileText);
			Console.WriteLine ("Hash = {0}", hash);
			Thread.Sleep (10000);
			File.WriteAllText (fileToCompress.FullName + ".gz.md5", hash);
		}

		public static void Decompress(FileInfo fileToDecompress)
		{
			using (FileStream originalFileStream = fileToDecompress.OpenRead())
			{
				string currentFileName = fileToDecompress.FullName;
				string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

				using (FileStream decompressedFileStream = File.Create(newFileName))
				{
					using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
					{
						decompressionStream.CopyTo(decompressedFileStream);
						Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
					}
				}
			}
			}
	}
}

