using System.Collections.Generic;

namespace Anil.Data.Mapping
{
    public partial class AnilEntityDescriptor
    {
        public AnilEntityDescriptor()
        {
            Fields = new List<AnilEntityFieldDescriptor>();
        }

        public string EntityName { get; set; }
        public string SchemaName { get; set; }
        public ICollection<AnilEntityFieldDescriptor> Fields { get; set; }
    }
}