using System;

namespace Recipes.Common
{
    public abstract class Disposable : IDisposable
    {
        protected bool disposed = false;

        protected abstract void DisposeAction();

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    DisposeAction();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
