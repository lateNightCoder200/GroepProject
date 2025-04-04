namespace API.DataModel.Models
{
    public class Notes
    {

        public Guid Id = Guid.NewGuid();    

        public string UserId { get; set; }
        public string Name { get; set; }

        public string Note { get; set; }

    }
}
