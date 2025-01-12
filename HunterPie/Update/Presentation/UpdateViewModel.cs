using HunterPie.UI.Architecture;

namespace HunterPie.Update.Presentation;

internal class UpdateViewModel : ViewModel
{
    private long _downloadedBytes;
    public long DownloadedBytes { get => _downloadedBytes; set => SetValue(ref _downloadedBytes, value); }

    private long _totalBytes;
    public long TotalBytes { get => _totalBytes; set => SetValue(ref _totalBytes, value); }

    private string _state = string.Empty;
    public string State { get => _state; set => SetValue(ref _state, value); }
}