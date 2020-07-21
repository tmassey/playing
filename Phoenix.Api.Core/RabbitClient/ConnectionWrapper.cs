using Phoenix.Api.Core.RabbitClient.Interfaces;
using RabbitMQ.Client;

namespace Phoenix.Api.Core.RabbitClient
{
    public class ConnectionWrapper: IConnectionWrapper
    {

        private readonly IConnection _connection;

        public ConnectionWrapper(IConnection connection)
        {
            _connection = connection;
        }

        public IModelWrapper CreateModel()
        {
            return new ModelWrapper(_connection.CreateModel());
        }

        public bool IsOpen => _connection.IsOpen;

        public void Close()
        {
            _connection.Close();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
