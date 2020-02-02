using System;
using System.Collections.Generic;
using AutoMapper;

namespace MoneyBook.Services {
    public abstract class ServiceBase : IDisposable {
        private static readonly Lazy<IMapper> mapper = new Lazy<IMapper>(() => {
            MapperConfiguration config = new MapperConfiguration(cfg => {
                cfg.AddProfile(new ServiceProfile());
            });
            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        });

        private readonly IList<IDisposable> disposableObjects = new List<IDisposable>();

        protected IMapper Mapper => mapper.Value;

        protected void AddDisposableObject(IDisposable disposable) {
            if (disposable != null) {
                disposableObjects.Add(disposable);
            }
        }

        #region IDisposable Support
        private bool disposed = false;

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void AddDisposableObject(params IDisposable[] objects) {
            foreach (IDisposable disposableObj in objects) {
                disposableObjects.Add(disposableObj);
            }
        }

        protected virtual void Dispose(bool disposing) {
            if (disposed) {
                return;
            }

            if (disposing) {
                foreach (var disposableObj in disposableObjects) {
                    disposableObj?.Dispose();
                }
            }

            disposed = true;
        }
        #endregion
    }
}
