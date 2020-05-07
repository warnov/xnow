using System.Collections.Generic;

namespace WarNov.xnow.WinFormsClient
{
    public class Executable
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Extension { get; set; }

        public bool InList(List<Executable> list)
        {
            foreach (var executable in list)
            {
                if (executable.Name == this.Name)
                {
                    this.Extension = executable.Extension;
                    this.FilePath = executable.FilePath;
                    return true;
                }                
            }
            return false;
        }
    }
}
