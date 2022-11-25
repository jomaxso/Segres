
<!-- PROJECT LOGO -->
<br />
<br />
<div align="center">
  <a href="https://github.com/othneildrew/Best-README-Template">
    <img src="https://raw.githubusercontent.com/amitmerchant1990/electron-markdownify/master/app/img/markdownify.png" alt="Segres" width="125">
  </a>

<h2 align="center">SEGRES</h2>

  <p align="center">
    The simple way to segerate your responsibilities
    <br />
    with Commands and Queries.
<br />
<br />
    <a href="https://github.com/othneildrew/Best-README-Template"><strong>Explore the docs »</strong></a>
<br />
<br />
  </p>

  <p align="center">
    <a href="https://github.com/othneildrew/Best-README-Template">View Demo</a>
    ·
    <a href="https://github.com/othneildrew/Best-README-Template/issues">Report Bug</a>
    ·
    <a href="https://github.com/othneildrew/Best-README-Template/issues">Request Feature</a>
  </p>

<p align="center">
      <a href="https://badge.fury.io/js/electron-markdownify">
        <img src="https://badge.fury.io/js/electron-markdownify.svg"
             alt="Gitter">
      </a>
      <a href="https://gitter.im/amitmerchant1990/electron-markdownify"><img src="https://badges.gitter.im/amitmerchant1990/electron-markdownify.svg"></a>
      <a href="https://saythanks.io/to/bullredeyes@gmail.com">
          <img src="https://img.shields.io/badge/SayThanks.io-%E2%98%BC-1EAEDB.svg">
      </a>
      <a href="https://www.paypal.me/AmitMerchant">
        <img src="https://img.shields.io/badge/$-donate-ff69b4.svg?maxAge=2592000&amp;style=flat">
      </a>
    </p>

</div>

<!-- ABOUT THE PROJECT -->

## About The Project


<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- GETTING STARTED -->

## Getting Started

This is an example of how you may give instructions on setting up your project locally.
To get a local copy up and running follow these simple example steps.

### Prerequisites


<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- USAGE EXAMPLES -->

## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

_For more examples, please refer to the [Documentation](https://example.com)_

#### IHandlerContext
| #   | Method                                                   | ReturnType                | ObjectType         | HandlerType                        |
|-----|----------------------------------------------------------|---------------------------|--------------------|------------------------------------|
| 1   | ```SendAsync(Command, CancellationToken)```              | ```Task```                | ```ICommand```     | ```ICommandHandler<T>```           |
| 2   | ```SendAsync(Command, CancellationToken)```              | ```Task<T>```             | ```ICommand<T>```  | ```ICommandHandler<TCommand, T>``` |
| 3   | ```SendAsync(Query, CancellationToken)```                | ```Task<T>```             | ```IQuery<T>```    | ```IQueryHandler<TQuery, T>```     |
| 4   | ```PublishAsync(Message, CancellationToken)```           | ```Task```                | ```IMessage<T>```  | ```IMessageHandler<T>```           |
| 5   | ```PublishAsync(Message, Strategy, CancellationToken)``` | ```Task```                | ```IMessage<T>```  | ```IMessageHandler<T>```           |
| 6   | ```CreateStreamAsync(Stream , CancellationToken)```      | ```IAsyncEnumerable<T>``` | ```IStream<T>```   | ```IStreamHandler<TStream, T>```   |


#### ISender


| #   | Method                                      | ReturnType    | ObjectType        | HandlerType                        |
|-----|---------------------------------------------|---------------|-------------------|------------------------------------|
| 1   | ```SendAsync(Command, CancellationToken)``` | ```Task```    | ```ICommand```    | ```ICommandHandler<T>```           |
| 2   | ```SendAsync(Command, CancellationToken)``` | ```Task<T>``` | ```ICommand<T>``` | ```ICommandHandler<TCommand, T>``` |
| 3   | ```SendAsync(Query, CancellationToken)```   | ```Task<T>``` | ```IQuery<T>```   | ```IQueryHandler<TQuery, T>```     |

#### IPublisher

| #   | Method                                                   | ReturnType | ObjectType        | HandlerType              |
|-----|----------------------------------------------------------|------------|-------------------|--------------------------|
| 1   | ```PublishAsync(Message, CancellationToken)```           | ```Task``` | ```IMessage<T>``` | ```IMessageHandler<T>``` |
| 2   | ```PublishAsync(Message, Strategy, CancellationToken)``` | ```Task``` | ```IMessage<T>``` | ```IMessageHandler<T>``` |

#### IStreamer

| #   | Method                                                                          | ReturnType                | ObjectType       | HandlerType                      |
|-----|---------------------------------------------------------------------------------|---------------------------|------------------|----------------------------------|
| 1   | ```CreateStreamAsync(Stream , CancellationToken)```                             | ```IAsyncEnumerable<T>``` | ```IStream<T>``` | ```IStreamHandler<TStream, T>``` |

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->

## Roadmap

- [x] Add Changelog
- [x] Add back to top links
- [ ] Add Additional Templates w/ Examples
- [ ] Add "components" document to easily copy & paste sections of the readme
- [ ] Multi-language Support
    - [ ] Chinese
    - [ ] Spanish

See the [open issues](https://github.com/othneildrew/Best-README-Template/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->

## Contact

Your Name - [@your_twitter](https://twitter.com/your_username) - email@example.com

Project Link: [https://github.com/your_username/repo_name](https://github.com/your_username/repo_name)

<p align="right">(<a href="#readme-top">back to top</a>)</p>




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/othneildrew/Best-README-Template.svg?style=for-the-badge
