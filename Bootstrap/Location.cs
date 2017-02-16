namespace BWakaBats.Bootstrap
{
    public struct Location : ILocation
    {
        public Location(double? latitude, double? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Location))
                return false;

            return Equals((Location)obj);
        }

        public bool Equals(Location other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public static bool operator ==(Location location1, Location location2)
        {
            return location1.Equals(location2);
        }

        public static bool operator !=(Location location1, Location location2)
        {
            return !location1.Equals(location2);
        }
    }
}
