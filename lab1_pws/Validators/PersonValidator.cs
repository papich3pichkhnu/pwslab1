using FluentValidation;
using lab1_pws.Models;
using System;
using System.Linq;

namespace lab1_pws.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10).NotNull();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Age).InclusiveBetween(18, 60);
            RuleFor(x => x.Job).Length(4, 20);
           
        }
        
    }
}
