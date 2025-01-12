using System;

namespace HunterPie.Core.Networking.Http.Events;
public class DownloadEventArgs : EventArgs
{

    public long BytesDownloaded { get; }
    public long TotalBytes { get; }

    public DownloadEventArgs(long bytesDownloaded, long totalBytes)
    {
        BytesDownloaded = bytesDownloaded;
        TotalBytes = totalBytes;
    }
}