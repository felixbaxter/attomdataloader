﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace attomdataloader
{
  public class TaxAssessor
  {
    public int RTPropertyUniqueIdentifier { get; set; }
    public long sa_property_id { get; set; }
    public string SitusStateCountyFIPS { get; set; }
    public string JurisdictionCountyFIPS { get; set; }
    public string APNUnformatted { get; set; }
    public string APNFormatted { get; set; }
    public string SitusAddress { get; set; }
    public string SitusCity { get; set; }
    public string SitusState { get; set; }
    public string SitusZip { get; set; }
    public string SitusZip4 { get; set; }
    public string SitusCounty { get; set; }
    public string SitusHouseNumber { get; set; }
    public string SitusHouseNumberFraction { get; set; }
    public string SitusStreetName { get; set; }
    public string SitusDirection { get; set; }
    public string SitusAddressSuffix { get; set; }
    public string SitusPostDirection { get; set; }
    public string SitusUnitPrefix { get; set; }
    public string SitusUnitValue { get; set; }
    public string CombinedStatisticalArea { get; set; }
    public string MetropolitanDivision { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public string OccupancyStatus { get; set; }
    public string LegalDescription { get; set; }
    public string LotNumber { get; set; }
    public string Subdivision { get; set; }
    public string Section { get; set; }
    public string Township { get; set; }
    public string Quarter { get; set; }
    public string Range { get; set; }
    public string PropertyZoning { get; set; }
    public string PropertyGroup { get; set; }
    public string PropertyType { get; set; }
    public int Bedrooms { get; set; }
    public decimal Bathrooms { get; set; }
    public int SquareFootage { get; set; }
    public decimal LotSize { get; set; }
    public int YearBuilt { get; set; }
    public int EffectiveYearBuilt { get; set; }
    public string ArchitectureDescription { get; set; }
    public string StructureDescription { get; set; }
    public string ExteriorDescription1 { get; set; }
    public string ExteriorDescription2 { get; set; }
    public string ConstructionDescription { get; set; }
    public decimal ContructionQuality { get; set; }
    public int LotDepth { get; set; }
    public int LotWidth { get; set; }
    public int FinishSquareFeet1 { get; set; }
    public int FinishSquareFeet2 { get; set; }
    public int FinishSquareFeet3 { get; set; }
    public int FinishSquareFeet4 { get; set; }
    public int AdditionsSquareFeet { get; set; }
    public int AtticSquareFeet { get; set; }
    public int BasementSquareFeet { get; set; }
    public int GarageSquareFeet { get; set; }
    public string HeatingCooling { get; set; }
    public string HeatingDetailDescription { get; set; }
    public string CoolingDetailDescription { get; set; }
    public string FirePlaceDescription { get; set; }
    public string GarageCarport { get; set; }
    public int BathroomNumberQuarter { get; set; }
    public int BathroomNumberHalf { get; set; }
    public int BathroomNumberThreeQuarter { get; set; }
    public int BathroomNumberFull { get; set; }
    public int NumberOfUnits { get; set; }
    public string RoofTypeDescription { get; set; }
    public string AirConditioning { get; set; }
    public string PoolDescription { get; set; }
    public int TaxYear { get; set; }
    public int TaxAssessedValue { get; set; }
    public int TaxImprovementValue { get; set; }
    public int TaxLandValue { get; set; }
    public int TaxImprovementPercent { get; set; }
    public int TaxAmount { get; set; }
    public int TaxDelinquentYear { get; set; }
    public int FullCashValue { get; set; }
    public int CurrentLimitValue { get; set; }
    public int MarketValue { get; set; }
    public int TaxExemptionAmountHomeowner { get; set; }
    public int TaxExemptionAmountDisabled { get; set; }
    public int TaxExemptionAmountSenior { get; set; }
    public int TaxExemptionAmountVeteran { get; set; }
    public int TaxExemptionAmountWidow { get; set; }
    public int TaxExemptionAmountOther { get; set; }
    public string TaxBillMailingAddress { get; set; }
    public string TaxBillMailingCity { get; set; }
    public string TaxBillMailingState { get; set; }
    public string TaxBillMailingZip { get; set; }
    public string TaxBillMailingZip4 { get; set; }
    public string TaxBillMailingCounty { get; set; }
    public string TaxBillMailingFIPs { get; set; }
    public string TaxBillMailingHouseNumber { get; set; }
    public string TaxBillMailingHouseNumberFraction { get; set; }
    public string TaxBillMailingStreetName { get; set; }
    public string TaxBillMailingDirection { get; set; }
    public string TaxBillMailingAddressSuffix { get; set; }
    public string TaxBillMailingPostDirection { get; set; }
    public string TaxBillMailingUnitPrefix { get; set; }
    public string TaxBillMailingUnitValue { get; set; }
    public string PrimaryOwnerNamePrefix { get; set; }
    public string PrimaryOwnerFullName { get; set; }
    public string PrimaryOwnerFirstName { get; set; }
    public string PrimaryOwnerMiddleName { get; set; }
    public string PrimaryOwnerLastName { get; set; }
    public string PrimaryOwnerNameSuffix { get; set; }
    public string PrimaryOwnerOtherPartyName { get; set; }
    public string PrimaryOwnerSpouseFirstName { get; set; }
    public string PrimaryOwnerSpouseMiddleName { get; set; }
    public string PrimaryOwnerSpouseNameSuffix { get; set; }
    public string SecondaryOwnerFullName { get; set; }
    public string SecondaryOwnerFirstName { get; set; }
    public string SecondaryOwnerMiddleName { get; set; }
    public string SecondaryOwnerLastName { get; set; }
    public string SecondryOwnerNameSuffix { get; set; }
    public string SecondaryOwnerSpouseFirstName { get; set; }
    public string SecondaryOwnerSpouseMiddleName { get; set; }
    public string OwnershipVestingRelationDescription { get; set; }
    public string TrustDescription { get; set; }
    public string SecondOwnerTypeDescription { get; set; }
    public string OwnerTypeDescription { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime PublicationDate { get; set; }
    public string ProcessIndicator { get; set; }
    public int EstimatedValue { get; set; }
    public DateTime LastSaleDate { get; set; }
    public int LastSaleAmount { get; set; }
    public DateTime PriorSaleDate { get; set; }
    public int PriorSaleAmount { get; set; }
    public int OpenLoan1 { get; set; }
    public int OpenLoan2 { get; set; }
    public int OpenLoan3 { get; set; }
    public int Equity { get; set; }
    public DateTime CalculationDate { get; set; }
    public int NumberOfRooms { get; set; }
    public int NumberOfStories { get; set; }
    public string ViewDescription { get; set; }
    public string MunicipalityName { get; set; }
    public string InactiveParcelFlag { get; set; }
    public string ParcelNumberReference { get; set; }
    public string ParcelAccountNumber { get; set; }
    public string ParcelNumberAlternate { get; set; }
    public string ParcelNumberPrevious { get; set; }
    public string ParcelNumberYearChange { get; set; }
    public string ShellParcelFlag { get; set; }
    public string ApnAddedYear { get; set; }
    public string AddressCRRT { get; set; }
    public string OwnerOccupied { get; set; }
    public string UcUseCodeMuni { get; set; }
    public int CensusBlockGroup { get; set; }
    public int CensusTract { get; set; }
    public string GeoQualityCode { get; set; }
    public string MSACode { get; set; }
    public decimal NumberOfBathsDQ { get; set; }
    public int FinishSquareFeet { get; set; }
    public int SquareFeetAssessorTotal { get; set; }
    public decimal Acreage { get; set; }
    public string LotType { get; set; }
    public int StructureNumber { get; set; }
    public string FoundationDescription { get; set; }
    public string InteriorCode { get; set; }
    public string LandSlopeCode { get; set; }
    public string BuildingQualityClassCode { get; set; }
    public string ElectricAvailableCode { get; set; }
    public string Basement1Code { get; set; }
    public string PatioPorchDescription { get; set; }
    public string PatioPorchDeck1Code { get; set; }
    public int PatioSqureFeet { get; set; }
    public int PorchSquareFeet { get; set; }
    public string FuelDescription { get; set; }
    public int FirePlaceNumber { get; set; }
    public string GarageDescription { get; set; }
    public int GarageSpacesNumber { get; set; }
    public string GasAvailableCode { get; set; }
    public string SewerUsedCode { get; set; }
    public string WaterUsedCode { get; set; }
    public string NeighborhoodCode { get; set; }
    public string TopographyCode { get; set; }
    public int TaxYearAssessed { get; set; }
    public string AppraiseImprovementPercent { get; set; }
    public string MarketImprovementPercent { get; set; }
    public string AppraiseImprovementValue { get; set; }
    public string AppraiseLandValue { get; set; }
    public string PreviousAssessedValue { get; set; }
    public string MarketImprovementValue { get; set; }
    public string LandMarketValue { get; set; }
    public string AppraiseLandYear { get; set; }
    public string ExemptFlag7 { get; set; }
    public string MailingCarrierCode { get; set; }
    public string CompanyFlag { get; set; }
    public DateTime TransferDate { get; set; }
    public string DocumentNumberFormat { get; set; }
    public string MailingPrivacyCode { get; set; }
    public int TransferPrice { get; set; }
    //public int FileID { get; set; }
  }
}
