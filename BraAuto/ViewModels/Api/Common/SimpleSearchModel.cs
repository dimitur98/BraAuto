namespace BraAuto.ViewModels.Api.Common
{
    public class SimpleSearchModel
    {
        public SimpleSearchModel(uint id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public uint Id { get; set; }

        public string Name { get; set; }
    }
}
