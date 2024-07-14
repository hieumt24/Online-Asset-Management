namespace AssetManagement.Domain.Enums
{
    public enum AssetStateType
    {
        Available = 1,
        NotAvailable = 2,
        Assigned = 3,
        WaitingForRecycling = 4,
        Recycled = 5
    }
}