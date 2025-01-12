using HunterPie.Integrations.Poogie.Patch;
using HunterPie.Integrations.Poogie.Patch.Models;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.Features.Patches.ViewModels;

internal class PatchesViewModel : ViewModel
{
    private readonly PoogiePatchConnector _patchConnector;

    private bool _isFetching;
    public bool IsFetching { get => _isFetching; set => SetValue(ref _isFetching, value); }

    public ObservableCollection<PatchViewModel> Patches { get; } = new();

    public PatchesViewModel(PoogiePatchConnector patchConnector)
    {
        _patchConnector = patchConnector;
    }

    public async Task FetchPatchesAsync()
    {
        IsFetching = true;
        (PatchResponse[]? patchNotes, _) = await _patchConnector.FindAll();
        IsFetching = false;

        if (patchNotes is null)
            return;

        await UIThread.InvokeAsync(() =>
        {
            foreach (PatchResponse note in patchNotes)
                Patches.Add(new PatchViewModel
                {
                    Title = note.Title,
                    Description = note.Description,
                    Banner = note.Banner,
                    Link = note.Link
                });
        });
    }
}