namespace MattsWorld.TellUs.Events.Journeys.Energy
{
    public class EnergyCustomerCreated : TellUsEvent
    {
        public override string Category => EventCategories.Customer;
        public override string Area => EventAreas.Energy;

        public string Message { get; set; }
    }
}
