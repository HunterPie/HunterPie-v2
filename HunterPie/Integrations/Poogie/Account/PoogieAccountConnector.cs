using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using System.Threading.Tasks;
using LoginRequest = HunterPie.Integrations.Poogie.Account.Models.LoginRequest;
using LoginResponse = HunterPie.Integrations.Poogie.Account.Models.LoginResponse;
using LogoutResponse = HunterPie.Integrations.Poogie.Account.Models.LogoutResponse;
using MyUserAccountResponse = HunterPie.Integrations.Poogie.Account.Models.MyUserAccountResponse;

namespace HunterPie.Integrations.Poogie.Account;

internal class PoogieAccountConnector
{
    private readonly IPoogieClientAsync _client;

    private const string ACCOUNT_ENDPOINT = "/v1/account";
    private const string USER_ENDPOINT = "/v1/user";
    private const string LOGIN_ENDPOINT = "/v1/login";
    private const string LOGOUT_ENDPOINT = "/v1/logout";

    public PoogieAccountConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<LoginResponse>> LoginAsync(LoginRequest request) =>
        await _client.PostAsync<LoginRequest, LoginResponse>(LOGIN_ENDPOINT, request);

    public async Task<PoogieResult<LogoutResponse>> LogoutAsync() =>
        await _client.PostAsync<Nothing, LogoutResponse>(LOGOUT_ENDPOINT, new Nothing());

    public async Task<PoogieResult<MyUserAccountResponse>> MyUserAccountAsync() =>
        await _client.GetAsync<MyUserAccountResponse>(USER_ENDPOINT + "/me");

    public async Task<PoogieResult<RegisterResponse>> RegisterAsync(RegisterRequest request) =>
        await _client.PostAsync<RegisterRequest, RegisterResponse>(ACCOUNT_ENDPOINT, request);

    public async Task<PoogieResult<MyUserAccountResponse>> UploadAvatarAsync(string filename) =>
        await _client.SendFileAsync<MyUserAccountResponse>(USER_ENDPOINT + "/avatar/upload", filename);

    public async Task<PoogieResult<PasswordChangeResponse>> ForgotPasswordAsync(PasswordResetRequest request) =>
        await _client.PostAsync<PasswordResetRequest, PasswordChangeResponse>($"{ACCOUNT_ENDPOINT}/password/reset", request);

    public async Task<PoogieResult<PasswordChangeResponse>> ChangePasswordAsync(ChangePasswordRequest request) =>
        await _client.PostAsync<ChangePasswordRequest, PasswordChangeResponse>($"{ACCOUNT_ENDPOINT}/password", request);

    public async Task<PoogieResult<RequestAccountVerificationResponse>> RequestAccountVerificationAsync(RequestAccountVerifyRequest request) =>
        await _client.PostAsync<RequestAccountVerifyRequest, RequestAccountVerificationResponse>($"{ACCOUNT_ENDPOINT}/verify", request);

}