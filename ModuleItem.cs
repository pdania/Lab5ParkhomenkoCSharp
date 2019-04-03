using System.Diagnostics;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ModuleItem
    {
        public string ModuleName { get; private set; }
        public string FilePath { get; private set; }

        public ModuleItem(ProcessModule module)
        {
            ModuleName = module.ModuleName;
            try
            {
                FilePath = module.FileName;
            }
            catch
            {

            }
        }
    }
}