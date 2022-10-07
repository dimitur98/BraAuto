namespace BraAuto.ViewModels.Common
{
    public class SimpleModel
    {
        public SimpleModel(uint id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public uint Id { get; set; }
        public string Name { get; set; }
    }
}
