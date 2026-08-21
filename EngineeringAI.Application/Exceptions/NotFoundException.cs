using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringAI.Application.Exceptions {
    public class NotFoundException : Exception {
        public NotFoundException(string message) : base(message) {
        }
    }
}
