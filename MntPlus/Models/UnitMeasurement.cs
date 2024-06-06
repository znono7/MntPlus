using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public enum UnitMeasurement
    {

        // Mesures Électriques
        Volts,
        Ampères,
        Ohms,
        Watts,
        KilowattHeures, // kWh

        // Mesures de Température
        Celsius,
        Fahrenheit,
        Kelvin,

        // Mesures de Longueur et Distance
        Mètres,
        Kilomètres,
        Centimètres,
        Millimètres,
        Miles,
        Pieds, // Feet
        Pouces, // Inches

        // Mesures de Volume
        Litres,
        Millilitres,
        MètresCubes, // Cubic meters
        Gallons,

        // Mesures de Pression
        Pascals,
        Kilopascals,
        Bar,
        PSI, // Pounds per square inch

        // Mesures de Débit
        LitresParSeconde,
        MètresCubesParHeure,
        GallonsParMinute,

        // Mesures de Poids et Masse
        Kilogrammes,
        Grammes,
        Tonnes,
        Livres, // Pounds

        // Mesures de Vitesse
        MètresParSeconde,
        KilomètresParHeure,
        MilesParHeure,

        // Mesures de Fréquence
        Hertz,
        Kilohertz,
        Mégahertz,
        Gigahertz,

        // Mesures de Temps
        Secondes,
        Minutes,
        Heures,

        // Mesures de Consommation de Carburant
        LitresPar100Kilomètres,
        MilesParGallon,

        // Autres Mesures
        Degrés,
        Radians,
        Candela,
        Lux,
        Décibels,
        Pourcentage
    


}

    public enum FrequencyUnit
    {
        Heures,
        Jours,
        Semaines,
        Mois
    }

}
