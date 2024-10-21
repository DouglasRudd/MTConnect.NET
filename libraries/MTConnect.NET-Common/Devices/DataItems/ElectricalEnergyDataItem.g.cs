// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

// MTConnect SysML v2.3 : UML ID = _19_0_3_45f01b9_1580378218285_896491_1770

namespace MTConnect.Devices.DataItems
{
    /// <summary>
    /// Wattage used or generated by a component over an interval of time.
    /// </summary>
    public class ElectricalEnergyDataItem : DataItem
    {
        public const DataItemCategory CategoryId = DataItemCategory.SAMPLE;
        public const string TypeId = "ELECTRICAL_ENERGY";
        public const string NameId = "electricalEnergy";
             
        public const string DefaultUnits = Devices.Units.WATT_SECOND;     
        public new const string DescriptionText = "Wattage used or generated by a component over an interval of time.";
        
        public override string TypeDescription => DescriptionText;
        
        public override System.Version MinimumVersion => MTConnectVersions.Version12;       


        public ElectricalEnergyDataItem()
        {
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
              
            Units = DefaultUnits;
        }

        public ElectricalEnergyDataItem(string deviceId)
        {
            Id = CreateId(deviceId, NameId);
            Category = CategoryId;
            Type = TypeId;
            Name = NameId;
             
            Units = DefaultUnits;
        }
    }
}