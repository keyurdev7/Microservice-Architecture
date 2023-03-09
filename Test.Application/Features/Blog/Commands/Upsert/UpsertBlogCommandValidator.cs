using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Application.Features.Blog.Commands.Upsert
{
    public class UpsertBlogCommandValidator : AbstractValidator<UpsertBlogCommand>
    {
        public UpsertBlogCommandValidator()
        {
            RuleFor(p => p.BlogTitle)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();

            RuleFor(p => p.BlogDescription)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull();
        }
    }
}
