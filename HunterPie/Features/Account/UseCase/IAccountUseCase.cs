using HunterPie.Features.Account.Event;
using HunterPie.Features.Account.Model;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.UseCase;

internal interface IAccountUseCase
{
    event EventHandler<AccountLoginEventArgs> SignIn;
    event EventHandler<EventArgs> SignOut;
    event EventHandler<AccountLoginEventArgs> SessionStart;
    event EventHandler<AccountAvatarEventArgs> AvatarChange;

    Task<UserAccount?> GetAsync();
    Task<bool> IsValidSessionAsync();

    // TODO: Move all of this to another interface
    Task<PoogieResult<LoginResponse>?> LoginAsync(LoginRequest request);
    Task UploadAvatarAsync(string path);
    Task LogoutAsync();
}