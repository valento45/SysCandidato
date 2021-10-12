using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ExtensionMethods.Authorizathion
{
    public class Certificados : X509Certificate
    {
        private X509Store _store;

        public X509Store Store
        {
            get
            {
                if(_store == null)
                {
                    _store = new X509Store();
                    _store.AddRange(new X509Store(StoreLocation.LocalMachine).Certificates);

                    _store.AddRange(new X509Store(StoreLocation.CurrentUser).Certificates);                    
                }
                return _store;
            }
            set
            {
                _store = value;
            }
        }





    }
}
