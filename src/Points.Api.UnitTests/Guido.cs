using System;

namespace Points.Api.UnitTests
{
    public static class Guido
    {
        public static string New()
        {
            return Guid.NewGuid().ToString("D");
        }
    }
}