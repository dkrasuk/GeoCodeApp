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
using log4net;
using BusinessLayer;

namespace GeoCodeApp
{
    public class WorkflowApp
    {
        private readonly IAddressServices _addressServices;
        private readonly ICsvService _csvService;
        private readonly ICollateralServices _collateralService;
        private readonly IGeoCodeService _geoCodeService;
        private static ILog log = LogManager.GetLogger("LOGGER");
        private static int countOk = 0;
        private static int countError = 0;

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
            Console.WriteLine(new string('-', 20));

            var collaterals = await _csvService.ParseCollateralsCsvFile();
            Console.WriteLine($"Количество залогов в *.csv: {collaterals?.Count()}");

            SetLocationByAddress(collaterals);

            Console.WriteLine(new string('-', 20));
            Console.WriteLine($"Successfully: {countOk}   |   Error: {countError}   |   Total: {collaterals?.Count()}");
            Console.WriteLine($"Application finished in {DateTime.Now} ");
            Console.ReadKey();
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
                                if (address == null)
                                {
                                    Logger.Log.ErrorFormat($"{collateral.COLLATERALID.Replace(" ", "")}    -    {collateral.CREDITAGREEMENTID}    -    {collateral.TYPE}    -   {address}");
                                    countError++;
                                }
                                if (address != null)
                                {
                                    string addressBuild = ((address.Region != "") ? $"{address.Region}, " : "")
                                        + ((address.Settlement != "") ? $"{address.Settlement}, " : "")
                                        + ((address.Street != "") ? $"{address.Street} " : "")
                                        + ((address.Flat != "") ? $"{address.Flat}" : "");

                                    var locationDto = GetGeoCode(addressBuild, collateral.COLLATERALID, collateral.CREDITAGREEMENTID, collateral.TYPE);
                                    if (locationDto == null)
                                    {
                                        countError++;
                                    }
                                    if (locationDto != null)
                                    {
                                        locationDto.Address = addressBuild;
                                        locationDto.Type = collateral.TYPE;
                                        var location = Mapper.Map<Model.Location, CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location>(locationDto);
                                        try
                                        {
                                            _collateralService.PostLocationAsync(collateral.COLLATERALID, location);
                                            countOk++;
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            countError++;
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

                                var locationDto = GetGeoCode(addressBuild, collateral.COLLATERALID, collateral.CREDITAGREEMENTID, collateral.TYPE);
                                if (locationDto == null)
                                {
                                    countError++;
                                }

                                if (locationDto != null)
                                {
                                    locationDto.Address = addressBuild;
                                    locationDto.Type = collateral.TYPE;
                                    var location = Mapper.Map<Model.Location, CollateralService.ApiClient.Client.Models.Presentation.Requests.Collateral.Location>(locationDto);
                                    try
                                    {
                                        _collateralService.PostLocationAsync(collateral.COLLATERALID, location);
                                        countOk++;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        countError++;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        public Model.Location GetGeoCode(string address, string collateralId, string creditagreemId, string type)
        {
            string apiKey = ConfigurationManager.AppSettings["GoogleApiKey"];
            var data = _geoCodeService.GetGeoCode(address, collateralId, apiKey, creditagreemId, type);

            var addressDto = data?.ToObject<Model.Location>();
            return addressDto;

        }
    }
}
