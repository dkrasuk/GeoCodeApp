using BusinessLayer.Interface;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral;
using System.Configuration;
using AutoMapper;

namespace GeoCodeApp
{
    public class WorkflowApp
    {
        private readonly IAddressServices _addressServices;
        private readonly ICsvService _csvService;
        private readonly ICollateralServices _collateralService;
        private readonly IGeoCodeService _geoCodeService;

        public WorkflowApp(IAddressServices addressServices, ICsvService csvService, ICollateralServices collateralService, IGeoCodeService geoCodeService)
        {
            _addressServices = addressServices;
            _csvService = csvService;
            _collateralService = collateralService;
            _geoCodeService = geoCodeService;
        }

        public async Task Run()
        {
            Console.WriteLine($"Application started in {DateTime.Now} ");
            var address = await _addressServices.GetRegistrationAddresses(3147954);
            Console.WriteLine(TaskStatus.RanToCompletion);

            var collaterals = await _csvService.ParseCollateralsCsvFile();
            SetLocationByAddress(collaterals); 
        }

        public void SetLocationByAddress(List<CollateralCsvModel> collaterals)
        {
            foreach (var collateral in collaterals)
            {
                if (Enum.IsDefined(typeof(CollateralTypeEnum), collateral.TYPE))
                {
                    switch ((CollateralTypeEnum)Enum.Parse(typeof(CollateralTypeEnum), collateral.TYPE))
                    {
                        case CollateralTypeEnum.AUTO:
                        case CollateralTypeEnum.OTHER:
                            {
                                Console.WriteLine($"Движемое имущество:  {collateral.TYPE}  -  AgreementId: {collateral.CREDITAGREEMENTID}");
                                var address = _addressServices.GetRegistrationAddresses(int.Parse(collateral.CREDITAGREEMENTID)).Result;
                                if (address != null)
                                {
                                    string addressBuild = ((address.Region != "") ? $"{address.Region}, " : "")
                                        + ((address.Settlement != "") ? $"{address.Settlement}, " : "")
                                        + ((address.Street != "") ? $"{address.Street} " : "")
                                        + ((address.Flat != "") ? $"{address.Flat}" : "");

                                    var locationDto = GetGeoCode(addressBuild, collateral.COLLATERALID);

                                    if (locationDto != null)
                                    {
                                        locationDto.Address = addressBuild;
                                        locationDto.Type = collateral.TYPE;
                                        var location = Mapper.Map<Model.Location, CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location>(locationDto);
                                        try
                                        {
                                            _collateralService.PostLocationAsync(collateral.COLLATERALID, location);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                            break;
                        case CollateralTypeEnum.HOME:
                        case CollateralTypeEnum.FLAT:
                        case CollateralTypeEnum.COMMERCIAL:
                        case CollateralTypeEnum.LAND:
                            {
                                Console.WriteLine($"Недвижимое имущество:   {collateral.TYPE}  -  CollateralId: {collateral.COLLATERALID}");
                                string addressBuild = ((collateral.REGION != "") ? $"{collateral.REGION}, " : "")
                                    + ((collateral.DISTRICT != "") ? $"{collateral.DISTRICT}, " : "")
                                    + ((collateral.SETTLEMENTTYPE != "") ? $"{collateral.SETTLEMENTTYPE}, " : "")
                                    + ((collateral.SETTLEMENT != "") ? $"{collateral.SETTLEMENT}, " : "")
                                    + ((collateral.STREET != "") ? $"{collateral.STREET} " : "")
                                    + ((collateral.HOUSE != "") ? $"{collateral.HOUSE}" : "");

                                var locationDto = GetGeoCode(addressBuild, collateral.COLLATERALID);

                                if (locationDto != null)
                                {
                                    locationDto.Address = addressBuild;
                                    locationDto.Type = collateral.TYPE;
                                    var location = Mapper.Map<Model.Location, CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location>(locationDto);
                                    try
                                    {
                                        _collateralService.PostLocationAsync(collateral.COLLATERALID, location);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        public Model.Location GetGeoCode(string address, string collateralId)
        {
            string apiKey = ConfigurationManager.AppSettings["GoogleApiKey"];
            var data = _geoCodeService.GetGeoCode(address, collateralId, apiKey);

            var addressDto = data?.ToObject<Model.Location>();
            return addressDto;

        }
    }
}
