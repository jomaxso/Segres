# Welcome to Segres
Clean coding should be simple and quick but also type save.

## IMediator



### ISender


| #   | Method                                      | ReturnType    | ObjectType        | HandlerType                        |
|-----|---------------------------------------------|---------------|-------------------|------------------------------------|
| 1   | ```SendAsync(Command, CancellationToken)``` | ```Task```    | ```ICommand```    | ```ICommandHandler<T>```           |
| 2   | ```SendAsync(Command, CancellationToken)``` | ```Task<T>``` | ```ICommand<T>``` | ```ICommandHandler<TCommand, T>``` |
| 3   | ```SendAsync(Query, CancellationToken)```   | ```Task<T>``` | ```IQuery<T>```   | ```IQueryHandler<TQuery, T>```     |

### IPublisher

| #   | Method                                                   | ReturnType | ObjectType        | HandlerType              |
|-----|----------------------------------------------------------|------------|-------------------|--------------------------|
| 1   | ```PublishAsync(Message, CancellationToken)```           | ```Task``` | ```IMessage<T>``` | ```IMessageHandler<T>``` |
| 2   | ```PublishAsync(Message, Strategy, CancellationToken)``` | ```Task``` | ```IMessage<T>``` | ```IMessageHandler<T>``` |

### IStreamer

| #   | Method                                                                          | ReturnType                | ObjectType       | HandlerType                      |
|-----|---------------------------------------------------------------------------------|---------------------------|------------------|----------------------------------|
| 1   | ```CreateStreamAsync(Stream , CancellationToken)```                             | ```IAsyncEnumerable<T>``` | ```IStream<T>``` | ```IStreamHandler<TStream, T>``` |
| 2   | ```StreamAsync(Stream, Func<T, Task> , CancellationToken)```                    | ```Task```                | ```IStream<T>``` | ```IStreamHandler<TStream, T>``` |
| 3   | ```StreamAsync(Stream, Func<T, CancellationToken, Task> , CancellationToken)``` | ```Task```                | ```IStream<T>``` | ```IStreamHandler<TStream, T>``` |
