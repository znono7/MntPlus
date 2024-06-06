using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public static class UnitMesurmentExtension
    {
        
            public static string ToUnitMeasurementString(this UnitMeasurement unite)
            {
                switch (unite)
                {
                    case UnitMeasurement.Volts:
                        return "Volts";
                    case UnitMeasurement.Ampères:
                        return "Ampères";
                    case UnitMeasurement.Ohms:
                        return "Ohms";
                    case UnitMeasurement.Watts:
                        return "Watts";
                    case UnitMeasurement.KilowattHeures:
                        return "Kilowatt-heures";
                    case UnitMeasurement.Celsius:
                        return "Celsius";
                    case UnitMeasurement.Fahrenheit:
                        return "Fahrenheit";
                    case UnitMeasurement.Kelvin:
                        return "Kelvin";
                    case UnitMeasurement.Mètres:
                        return "Mètres";
                    case UnitMeasurement.Kilomètres:
                        return "Kilomètres";
                    case UnitMeasurement.Centimètres:
                        return "Centimètres";
                    case UnitMeasurement.Millimètres:
                        return "Millimètres";
                    case UnitMeasurement.Miles:
                        return "Miles";
                    case UnitMeasurement.Pieds:
                        return "Pieds";
                    case UnitMeasurement.Pouces:
                        return "Pouces";
                    case UnitMeasurement.Litres:
                        return "Litres";
                    case UnitMeasurement.Millilitres:
                        return "Millilitres";
                    case UnitMeasurement.MètresCubes:
                        return "Mètres cubes";
                    case UnitMeasurement.Gallons:
                        return "Gallons";
                    case UnitMeasurement.Pascals:
                        return "Pascals";
                    case UnitMeasurement.Kilopascals:
                        return "Kilopascals";
                    case UnitMeasurement.Bar:
                        return "Bar";
                    case UnitMeasurement.PSI:
                        return "PSI";
                    case UnitMeasurement.LitresParSeconde:
                        return "Litres par seconde";
                    case UnitMeasurement.MètresCubesParHeure:
                        return "Mètres cubes par heure";
                    case UnitMeasurement.GallonsParMinute:
                        return "Gallons par minute";
                    case UnitMeasurement.Kilogrammes:
                        return "Kilogrammes";
                    case UnitMeasurement.Grammes:
                        return "Grammes";
                    case UnitMeasurement.Tonnes:
                        return "Tonnes";
                    case UnitMeasurement.Livres:
                        return "Livres";
                    case UnitMeasurement.MètresParSeconde:
                        return "Mètres par seconde";
                    case UnitMeasurement.KilomètresParHeure:
                        return "Kilomètres par heure";
                    case UnitMeasurement.MilesParHeure:
                        return "Miles par heure";
                    case UnitMeasurement.Hertz:
                        return "Hertz";
                    case UnitMeasurement.Kilohertz:
                        return "Kilohertz";
                    case UnitMeasurement.Mégahertz:
                        return "Mégahertz";
                    case UnitMeasurement.Gigahertz:
                        return "Gigahertz";
                    case UnitMeasurement.Secondes:
                        return "Secondes";
                    case UnitMeasurement.Minutes:
                        return "Minutes";
                    case UnitMeasurement.Heures:
                        return "Heures";
                    case UnitMeasurement.LitresPar100Kilomètres:
                        return "Litres par 100 kilomètres";
                    case UnitMeasurement.MilesParGallon:
                        return "Miles par gallon";
                    case UnitMeasurement.Degrés:
                        return "Degrés";
                    case UnitMeasurement.Radians:
                        return "Radians";
                    case UnitMeasurement.Candela:
                        return "Candela";
                    case UnitMeasurement.Lux:
                        return "Lux";
                    case UnitMeasurement.Décibels:
                        return "Décibels";
                    case UnitMeasurement.Pourcentage:
                        return "Pourcentage";
                    default:
                        return "Unité inconnue";
                }
            }
        

    }
}
