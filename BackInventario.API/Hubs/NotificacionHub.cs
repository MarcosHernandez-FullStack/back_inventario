using Microsoft.AspNetCore.SignalR;

namespace BackInventario.API.Hubs;

public class NotificacionHub : Hub
{
    // Los clientes se unen al grupo según su rol al conectarse
    public async Task UnirseAlGrupo(string rol)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, rol);
    }
}
