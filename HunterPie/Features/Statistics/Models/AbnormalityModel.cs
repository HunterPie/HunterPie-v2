namespace HunterPie.Features.Statistics.Models;

internal record AbnormalityModel(
    string Id,
    TimeFrameModel[] Activations
);