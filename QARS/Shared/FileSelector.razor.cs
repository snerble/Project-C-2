using Tewr.Blazor.FileReader;

namespace QARS.Shared
{
	/// <summary>
	/// Contains the data from a finished file download.
	/// </summary>
	public struct FileDownloadResult
	{
		/// <summary>
		/// Initializes a new instance of <see cref="FileDownloadResult"/>.
		/// </summary>
		/// <param name="file">The info about the downloaded file.</param>
		/// <param name="data">The byte data of the file.</param>
		public FileDownloadResult(IFileInfo file, byte[] data)
		{
			File = file;
			Data = data;
		}

		/// <summary>
		/// Gets the <see cref="IFileInfo"/> of the downloaded file.
		/// </summary>
		public IFileInfo File { get; }
		/// <summary>
		/// Gets the downloaded byte data of the file.
		/// </summary>
		public byte[] Data { get; }
		/// <summary>
		/// Gets the Mime content type of the file.
		/// </summary>
		public string ContentType => MimeKit.MimeTypes.GetMimeType(File.Name);
	}
}
