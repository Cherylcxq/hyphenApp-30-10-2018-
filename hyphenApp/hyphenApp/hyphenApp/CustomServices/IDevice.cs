using System;
using Xamarin.Forms;

namespace hyphenApp
{
	public interface IDevice
	{
		string GetDocumentsFolder();
		string GetPublicFolder();
		string Encrypt(string inText, string key);
		string Decrypt(string cryptTxt,string key);
		string Hash (string key);
		string ReadFileIntoBase64String(string filePath);
		int GetImageRotationFromExif(string imagePath);
		void CopyFile(string sourceFilePath, string destinationFilePath);
		void MoveFile(string sourceFilePath, string destinationFilePath);
		void DeleteFile(string sourceFilePath);

		void SetWebViewLoadEventHandler(EventHandler<string> evt);
		void RateApplication();

		DisplayResolution GetDisplayResolution();

		void SetCulture(string cultureName);
	}


	public class DisplayResolution
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public double Scale { get; set; }
	}
}



