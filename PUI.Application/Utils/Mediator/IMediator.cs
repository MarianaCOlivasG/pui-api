




namespace PUI.Application.Utils.Mediator 
{

    public interface IMediator
    {
        Task<TResponse> Send<TResponse>( IRequest<TResponse> request );

        Task Send( IRequest request );


    }
    
}