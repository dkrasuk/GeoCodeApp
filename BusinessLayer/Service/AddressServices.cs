using BusinessLayer.Interface;
using DataAccessLayer.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressServices : IAddressServices
    {
        private readonly IAgreementService _agreementService;

        public AddressServices(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }
        public async Task<ClientAddresses> GetRegistrationAddresses(int agreementId)
        {
            var clientAddress = await _agreementService.GetRegistrationAddresses(agreementId);
            return clientAddress;
        }
    }
}
