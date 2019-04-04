using System.Diagnostics;

namespace Lab5ParkhomenkoCSharp2019
{
    public class ModuleItem
    {
        public string ModuleName { get;}
        public string FilePath { get;}

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