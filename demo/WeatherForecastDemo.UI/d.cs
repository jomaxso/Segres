﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Segres.Commons;
using Segres.Contracts;

namespace Segres;

public class ThePerson2 : IRequest
{
    public ThePerson2(int Age)
    {
        this.Age = Age;
    }

    public int Age { get; set; }
}

public class ThePerson : IRequest<Result<int>>
{
    public ThePerson(int Age)
    {
        this.Age = Age;
    }

    public int Age { get; set; }
}
