# Welcome to CequrS
Clean coding should be simple and quick but also type save.



| #   | Feature      | Object                                  | Handler                                                                   | Description |
|-----|--------------|-----------------------------------------|---------------------------------------------------------------------------|-------------|
| 1   | PublishAsync | ```IMessage<TResult>```                 | ```IMessageHandler<TMessage>```                                           ||
| 2   | CommandAsync | ```ICommand```, ```ICommand<TResult>``` | ```ICommandHandler<TCommand>```, ```ICommandHandler<TCommand, TResult>``` ||
| 3   | QueryAsync   | ```IQuery<TResult>```                   | ```IQueryHandler<TQuery, TResult>```                                      ||
| 4   | StreamAsync  | ```IStream<TResult>```                  | ```IStreamHandler<TStream, TResult>```                                    ||
