using System.Collections.Generic;
using Phoenix.Api.Core;
using Phoenix.Api.Core.Bootstrappers;
using playing.Bootstrappers;

namespace playing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Service.Start(new List<IBootstrapper>{new MyBootstrapper()});
        }
    }
}
