namespace WebApi.Models.DTO
{
    public class DTOBudgetWijziging
    {
        public float bedrag { get; set; }
        public string beschrijving { get; set; }
        public int inspraakItemId { get; set; }
        public string InspraakItem { get; set; }
    }
}