﻿using System;
using System.Runtime.Serialization;

namespace PlayTogether.Core.Exceptions
{
    [Serializable]
    public class NotFoundException : DomainException
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}