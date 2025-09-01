using System;
using System.Collections.Generic;
using System.Linq;
using Avvo.Core.Abstractions;

namespace Avvo.Domain.Entities
{
    public record Payment(string Method, decimal Amount);
}
