namespace API.Shared
{
    public enum USER_STATUS : int
    {
        active = 1,
        disabled = 2,
        pending = 3,
    }

    public enum USER_TYPE : int
    {
        admin = 1,
        operation = 2,
        provider = 3,
        customer = 4,
    }

    public enum PRICE_MODEL : int
    {
        perKM = 1,
        perHour = 2,
    }

    public enum ORDER_STATUS : int
    {
        created = 1,
        running = 2,
        ended = 2,
        paid = 3,
    }

    public enum VEHICLE_TYPE : int
    {
        motorcycle = 1,
        sedan = 2,
        luxury = 3,
        truck = 4,
    }

    public enum VEHICLE_STATUS : int
    {
        active = 1,
        disabled = 2,
        running = 3,
    }

    public class Roles
    {
        public const string Users = "1";
    }

}
