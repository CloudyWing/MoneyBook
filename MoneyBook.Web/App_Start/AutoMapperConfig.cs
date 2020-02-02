using System.Reflection;
using AutoMapper;

namespace MoneyBook.Web {
    public static class AutoMapperConfig {
        public static MapperConfiguration Create() {
            return new MapperConfiguration(cfg => {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });
        }
    }
}
