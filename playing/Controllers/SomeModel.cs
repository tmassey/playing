using Swashbuckle.AspNetCore.Annotations;

namespace playing.Controllers
{
    public class SomeModel{
        [SwaggerSchema("The product id",ReadOnly = true)]
        public int id { get; set; }

        [SwaggerSchema("The product description")]
        public string desc { get; set; }
    }
}