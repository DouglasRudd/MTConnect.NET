// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MTConnect.Streams.Samples
{
    /// <summary>
    /// The density of the material deposited in an additive manufacturing process per unit of volume.
    /// </summary>
    public class DepositionDensityValue : SampleValue
    {
        protected override double MetricConversion => 1;
        protected override double InchConversion => 1;
        protected override string MetricUnits => "MILLIGRAM/CUBIC_MILLIMETER";
        protected override string InchUnits => "MILLIGRAM/CUBIC_MILLIMETER";


        public DepositionDensityValue(double density, UnitSystem unitSystem = UnitSystem.METRIC)
        {
            Value = density;
            UnitSystem = unitSystem;
        }
    }
}