namespace PioneerLiganSTHLM.ViewModels
{
    public class League
    {
        public List<Models.Player> Players { get; set; } = new List<Models.Player>();
        public List<Models.Event> Events { get; set; } = new List<Models.Event>();
    }
}
