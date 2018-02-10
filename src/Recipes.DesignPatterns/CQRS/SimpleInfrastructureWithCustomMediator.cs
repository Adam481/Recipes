using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Recipes.DesignPatterns.CQRS
{
    class SimpleInfrastructureWithCustomMediator
    {
        class Client
        {
            public Guid Id { get; private set; }
            public string Name { get; private set; }
            public string Surname { get; private set; }

            public Client(Guid id, string name, string surname)
            {
                // Validate params
                Id = id;
                Name = name;
                Surname = surname;
            }
        }


        interface IQuery<out TResult>
        {
            TResult Result { get; }
        }


        interface IQueryHandler<TQuery>
        {
            Task<TQuery> Handle(TQuery query);
        }


        interface ICommand<out TResult>
        {
            TResult Result { get; }
        }


        interface ICommandHandler<TCommand>
        {
            Task<TCommand> Handle(TCommand command);
        }


        class GetClientByIdQuery : IQuery<Client>
        {
            public Guid ClientId { get; }
            public Client Result { get; }

            public GetClientByIdQuery(Client result)
            {
                Result = result ?? throw new ArgumentNullException($"{nameof(result)} cannot be null");
            }

            public GetClientByIdQuery(Guid clientId)
            {
                // validate
                ClientId = clientId;
            }
        }


        public class CreateClientCommand : ICommand<Guid>
        {
            public Guid Result { get; protected set; }
            public string ClientName { get; protected set; }
            public string ClientSurname { get; protected set; }

            public CreateClientCommand(string clientName, string clientSurname)
            {
                // Validate
                ClientName = clientName;
                ClientSurname = clientSurname;
            }

            public CreateClientCommand(Guid result)
            {
                // Validate
                Result = result;
            }
        }


        //class ClientService :
        //    IQueryHandler<GetClientByIdQuery>,
        //    ICommandHandler<CreateClientCommand>
        //{
        //    public async Task<GetClientByIdQuery> Handle(GetClientByIdQuery query)
        //    {
        //        var client = await _clientRepository.GetAsync(query.ClientId);
        //        return new GetClientByIdQuery(client);
        //    }


        //    public async Task<CreateClientCommand> Handle(CreateClientCommand command)
        //    {
        //        var client = await ClientFactory.CreateInstanceAsync(command);
        //        _clientRepository.Create(client);
        //        return new CreateClientCommand(client);
        //    }
        //}


        public interface IMediator
        {
            Task<TCommand> Send<TCommand>(TCommand command);
            Task<TQuery> Request<TQuery>(TQuery query);
        }

        delegate object HandlerFactory(Type type);

        //class Mediator : IMediator
        //{
        //    private readonly ConcurrentDictionary<Type, object> _handlers = new ConcurrentDictionary<Type, object>();
        //    private readonly HandlerFactory _factory;

        //    public Mediator(HandlerFactory factory)
        //    {
        //        _factory = factory ?? throw new ArgumentNullException($"{nameof(factory)} cannot be null");
        //    }

        //    public async Task<TCommand> Send<TCommand>(TCommand command)
        //    {
        //        // TODO:
        //        var commandType = command.GetType();
        //        var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        //        return await GetCommandHandler(command, handlerType).Handle(command);
        //    }


        //    public async Task<TQuery> Request<TQuery>(TQuery query)
        //    {
        //        // TODO:
        //        var queryType = query.GetType();
        //        var handlerType = typeof(IQueryHandler<>).MakeGenericType(queryType);

        //        return await GetQueryHandler(query, handlerType).Handle(query);
        //    }

        //}

        // TODO: add sample repository
        // TODO: add autofac registration for all types
        // TODO: add usage sample
        //var response = await _mediator.Request(new GetClientByIdQuery(clientId));
        //return new JsonResult(response.Result);

    }
}
