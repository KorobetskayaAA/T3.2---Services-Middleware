namespace CatsWepApiWithDb.Controllers.Model
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contacts { get; set; }

        public static Owner Map(DAL.Owner owner)
        {
            return new Owner()
            {
                Id = owner.Id,
                Name = owner.Name,
                Contacts = owner.Contacts,
            };
        }

        public static DAL.Owner Map(Owner owner)
        {
            return new DAL.Owner()
            {
                Id = owner.Id,
                Name = owner.Name,
                Contacts = owner.Contacts,
            };
        }
    }
}
