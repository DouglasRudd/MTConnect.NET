﻿// Copyright (c) 2022 TrakHound Inc., All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.

using MTConnect.Devices;
using MTConnect.Observations.Input;
using System;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MTConnect.Observations
{
    /// <summary>
    /// An XML element which provides the information and data reported from a piece of equipment for those DataItem elements defined with a category attribute of EVENT in the MTConnectDevices document.
    /// </summary>
    public class EventObservation : Observation, IEventObservation
    {
        /// <summary>
        /// For those DataItem elements that report data that may be periodically reset to an initial value, 
        /// resetTriggered identifies when a reported value has been reset and what has caused that reset to occur.
        /// </summary>
        [XmlAttribute("resetTriggered")]
        [JsonPropertyName("resetTriggered")]
        public ResetTriggered ResetTriggered
        {
            get => GetValue(ValueKeys.ResetTriggered).ConvertEnum<ResetTriggered>();
            set => AddValue(new ObservationValue(ValueKeys.ResetTriggered, value));
        }

        internal bool ResetTriggeredOutput => ResetTriggered != ResetTriggered.NOT_SPECIFIED;


        public EventObservation()
        {
            SetProperty("Category", DataItemCategory.EVENT);
        }


        /// <summary>
        /// Determine if the DataItem with the specified Observation is valid in the specified MTConnectVersion
        /// </summary>
        /// <param name="mtconnectVersion">The Version of the MTConnect Standard</param>
        /// <param name="observation">The Observation to validate</param>
        /// <returns>A DataItemValidationResult indicating if Validation was successful and a Message</returns>
        protected ObservationValidationResult Validate<T>(Version mtconnectVersion, IObservationInput observation) where T : struct
        {
            if (observation != null && !observation.Values.IsNullOrEmpty())
            {
                // Get the CDATA Value for the Observation
                var cdata = observation.GetValue(ValueKeys.CDATA);
                if (cdata != null)
                {
                    // Check Valid values in Enum
                    var validValues = Enum.GetValues(typeof(T));
                    foreach (var validValue in validValues)
                    {
                        if (cdata == validValue.ToString())
                        {
                            return new ObservationValidationResult(true);
                        }
                    }

                    return new ObservationValidationResult(false, "'" + cdata + "' is not a valid value");
                }
                else
                {
                    return new ObservationValidationResult(false, "No CDATA is specified for the Observation");
                }
            }

            return new ObservationValidationResult(false, "No Observation is Specified");
        }


        public static EventObservation Create(DataItem dataItem)
        {
            if (dataItem != null)
            {
                var observation = Create(dataItem.Type, dataItem.Representation);
                observation.SetProperty(nameof(DataItemId), dataItem.Id);
                observation.SetProperty(nameof(Representation), dataItem.Representation);
                observation.SetProperty(nameof(Type), dataItem.Type);
                observation.SetProperty(nameof(SubType), dataItem.SubType);
                observation.SetProperty(nameof(Name), dataItem.Name);
                observation.SetProperty(nameof(CompositionId), dataItem.CompositionId);
                return observation;
            }

            return null;
        }

        public static EventObservation Create(string type, DataItemRepresentation representation)
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (_types == null) _types = GetAllTypes();

                if (!_types.IsNullOrEmpty())
                {
                    var key = $"{type.ToPascalCase()}{representation.ToString().ToPascalCase()}";

                    if (_types.TryGetValue(key, out Type t))
                    {
                        var constructor = t.GetConstructor(System.Type.EmptyTypes);
                        if (constructor != null)
                        {
                            try
                            {
                                switch (representation)
                                {
                                    case DataItemRepresentation.VALUE: return (EventValueObservation)Activator.CreateInstance(t);
                                    case DataItemRepresentation.DATA_SET: return (EventDataSetObservation)Activator.CreateInstance(t);
                                    case DataItemRepresentation.TABLE: return (EventTableObservation)Activator.CreateInstance(t);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }

            switch (representation)
            {
                case DataItemRepresentation.DATA_SET: return new EventDataSetObservation();
                case DataItemRepresentation.TABLE: return new EventTableObservation();
                default: return new EventValueObservation();
            }
        }
    }
}