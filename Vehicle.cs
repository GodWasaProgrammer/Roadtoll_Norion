namespace Roadtoll_Norion
{
    /// <summary>
    /// Realized i probably shouldnt rename this
    /// However, the convention states it should have an I before an interface
    /// So thats why in my opinion this should be named IVehicle
    /// I have also intentionally not modified this,
    /// as i cannot predict what else is using this and thus would break if i renamed it
    /// or modified it.
    /// </summary>
    public interface Vehicle
    {
        string GetVehicleType();
    }

}