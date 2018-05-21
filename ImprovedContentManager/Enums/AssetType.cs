namespace ImprovedContentManager.Enums
{
    public enum AssetType
    {
        [AssetType(null, null)]
        All,
        [AssetType(null, null)]
        Unknown,
        [AssetType(SteamHelper.kSteamTagBuilding, "InfoIconOutsideConnectionsPressed")]
        Building,
        [AssetType(SteamHelper.kSteamTagResidential, "InfoIconOutsideConnectionsPressed")]
        Residential,
        [AssetType(SteamHelper.kSteamTagResidentialLow, "IconPolicyTaxRaiseResLow")]
        ResidentialLow,
        [AssetType(SteamHelper.kSteamTagResidentialHigh, "IconPolicyTaxRaiseResHigh")]
        ResidentialHigh,
        [AssetType(SteamHelper.kSteamTagCommercial, "InfoIconOutsideConnectionsPressed")]
        Commercial,
        [AssetType(SteamHelper.kSteamTagCommercialLow, "IconPolicySmallBusiness")]
        CommercialLow,
        [AssetType(SteamHelper.kSteamTagCommercialHigh, "IconPolicyBigBusiness")]
        CommercialHigh,
        [AssetType(SteamHelper.kSteamTagCommercialLeisure, "IconPolicyLeisure")]
        CommercialLeisure,
        [AssetType(SteamHelper.kSteamTagCommercialTourist, "IconPolicyTourist")]
        CommercialTourist,
        [AssetType(SteamHelper.kSteamTagIndustrial, "InfoIconOutsideConnectionsPressed")]
        Industrial,
        [AssetType(SteamHelper.kSteamTagIndustrialGeneric, "IconPolicyNone")]
        IndustrialGeneric,
        [AssetType(SteamHelper.kSteamTagIndustrialOil, "IconPolicyOil")]
        IndustrialOil,
        [AssetType(SteamHelper.kSteamTagIndustrialOre, "IconPolicyOre")]
        IndustrialOre,
        [AssetType(SteamHelper.kSteamTagIndustrialForestry, "IconPolicyForest")]
        IndustrialForestry,
        [AssetType(SteamHelper.kSteamTagIndustrialFarming, "IconPolicyFarming")]
        IndustrialFarming,
        [AssetType(SteamHelper.kSteamTagOffice, "InfoIconOutsideConnectionsPressed")]
        Office,
        [AssetType(SteamHelper.kSteamTagProp, "ToolbarIconProps")]
        Prop,
        [AssetType(SteamHelper.kSteamTagTree, "ToolbarIconBeautification")]
        Tree,
        [AssetType(SteamHelper.kSteamTagIntersection, "ThumbnailJunctionsClover")]
        Intersection,
        [AssetType(SteamHelper.kSteamTagPark, "SubBarBeautificationOthersPressed")]
        Park,
        [AssetType(SteamHelper.kSteamTagElectricity, "InfoIconElectricity")]
        Electricity,
        [AssetType(SteamHelper.kSteamTagWaterAndSewage, "InfoIconWater")]
        WaterAndSewage,
        [AssetType(SteamHelper.kSteamTagGarbage, "InfoIconGarbage")]
        Garbage,
        [AssetType(SteamHelper.kSteamTagHealthcare, "ToolbarIconHealthcare")]
        Healthcare,
        [AssetType(SteamHelper.kSteamTagDeathcare, "ToolbarIconHealthcareDisabled")]
        Deathcare,
        [AssetType(SteamHelper.kSteamTagFireDepartment, "InfoIconFireSafety")]
        FireDepartment,
        [AssetType(SteamHelper.kSteamTagPoliceDepartment, "ToolbarIconPolice")]
        PoliceDepartment,
        [AssetType(SteamHelper.kSteamTagEducation, "InfoIconEducation")]
        Education,
        [AssetType(SteamHelper.kSteamTagTransport, "ToolbarIconPublicTransport")]
        Transport,
        [AssetType(SteamHelper.kSteamTagTransportBus, "SubBarPublicTransportBus")]
        TransportBus,
        [AssetType(SteamHelper.kSteamTagTransportMetro, "SubBarPublicTransportMetro")]
        TransportMetro,
        [AssetType(SteamHelper.kSteamTagTransportTrain, "SubBarPublicTransportTrain")]
        TransportTrain,
        [AssetType(SteamHelper.kSteamTagTransportShip, "SubBarPublicTransportShip")]
        TransportShip,
        [AssetType(SteamHelper.kSteamTagTransportPlane, "SubBarPublicTransportPlane")]
        TransportPlane,
        [AssetType(SteamHelper.kSteamTagTransportTaxi, "SubBarPublicTransportTaxi")]
        TransportTaxi,
        [AssetType(SteamHelper.kSteamTagTransportTram, "SubBarPublicTransportTram")]
        TransportTram,
        [AssetType(SteamHelper.kSteamTagTransportTram, "SubBarPublicTransportMonorail")]
        TransportMonorail,
        [AssetType(SteamHelper.kSteamTagTransportCableCar, "SubBarPublicTransportCableCar")]
        TransportCableCar,
        [AssetType(SteamHelper.kSteamTagUniqueBuilding, "ToolbarIconMonuments")]
        UniqueBuilding,
        [AssetType(SteamHelper.kSteamTagMonument, "ToolbarIconWonders")]
        Monument,
        [AssetType(SteamHelper.kSteamTagColorCorrectionLUT, null)]
        ColorLut,
        [AssetType(SteamHelper.kSteamTagVehicle, "InfoIconTrafficCongestion")]
        Vehicle,
        [AssetType(SteamHelper.kSteamTagCitizen, "InfoIconPopulation")]
        Citizen,
        [AssetType(SteamHelper.kSteamTagRoad, "ToolbarIconRoads")]
        Road,
    }
}