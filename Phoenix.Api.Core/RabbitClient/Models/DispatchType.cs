namespace Phoenix.Api.Core.RabbitClient.Models
{
    public enum DispatchType
    {
        Direct,
        Topic,
        Fanout,
        Worker,
        None
    }
}