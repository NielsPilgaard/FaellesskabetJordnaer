using Jordnaer.Database;
using Jordnaer.Features.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;

namespace Jordnaer.SignalR;

public abstract class AuthenticatedSignalRClientBase(
	CurrentUser currentUser,
	ILogger<AuthenticatedSignalRClientBase> logger,
	UserManager<ApplicationUser> userManager,
	IServer server,
	NavigationManager navigationManager,
	string hubPath)
	: ISignalRClient
{
	protected bool Started { get; private set; }

	public bool IsConnected => HubConnection?.State is HubConnectionState.Connected;

	protected HubConnection? HubConnection { get; private set; }

	public async Task StartAsync(CancellationToken cancellationToken = default)
	{
		if (currentUser.Id is null)
		{
			logger.LogDebug("CurrentUser is not logged in, cannot create an authenticated SignalR Connection.");
			return;
		}

		var user = await userManager.FindByIdAsync(currentUser.Id);
		if (user?.Cookie is null)
		{
			logger.LogWarning("CurrentUser {UserId} does not have a cookie, cannot create an authenticated SignalR Connection.", currentUser.Id);
			return;
		}

		return;
		var cookieContainer = CreateCookieContainer(user);
		if (cookieContainer is null)
		{
			return;
		}

		HubConnection = new HubConnectionBuilder()
						.WithUrl(navigationManager.ToAbsoluteUri(hubPath),
								 options => options.Cookies = cookieContainer)
						.WithAutomaticReconnect()
						.Build();

		if (!Started && HubConnection is not null)
		{
			await HubConnection.StartAsync(cancellationToken);
			Started = true;
		}
	}

	private CookieContainer? CreateCookieContainer(ApplicationUser user)
	{
		var serverUri = server.Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
		if (serverUri is null)
		{
			logger.LogError("Failed to get server address from IServer");
			return null;
		}

		var domain = new Uri(serverUri).Host;

		var cookieContainer = new CookieContainer(1);
		cookieContainer.Add(new Cookie(AuthenticationConstants.CookieName, user.Cookie, "/", domain));

		return cookieContainer;
	}

	public async ValueTask DisposeAsync()
	{
		if (HubConnection is not null)
		{
			await HubConnection.DisposeAsync();
		}

		GC.SuppressFinalize(this);
	}
}
