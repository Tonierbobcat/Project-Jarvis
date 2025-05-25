namespace ProjectJarvis.Core;

/**
 * {Temperature} Celsius
 * {AirQuality} AQI (American Air Quality Index)
 */
public class Weather(int Temperature, double Humidity, double PrecipitationChance, int AirQuality, int WindSpeed,
    int WindDirection, int UVIndex)
{
    public static void Validate(int Temperature, double Humidity, double PrecipitationChance, int AirQuality, int WindSpeed,
        int WindDirection, int UVIndex)
    {
        if (AirQuality < 0)
        {
            throw new ArgumentException("AirQuality must be greater than or equal to zero.");
        }
        if (Humidity is < 0 or > 1.0) 
        {
            throw new ArgumentException("Humidity must be between 0 and 1.");
        }
        if (UVIndex < 0)
        {
            throw new ArgumentException("UVIndex must be greater than or equal to zero.");
        }
        if (PrecipitationChance is < 0 or > 1.0) 
        {
            throw new ArgumentException("PrecipitationChance must be between 0 and 1.");
        }
    }
}