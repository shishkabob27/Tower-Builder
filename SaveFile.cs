using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tower_Builder
{
    internal class SaveFile
    {
        public bool TouchedSun;

        public int LeftoverRibbons;

        public List<int> TutorialSteps;

        public string SlotMessage;

        public string FileBlocks;

        public string FilePowers;

        public void Initialize(string filename1, string filename2)
        {
            FileBlocks = filename1;
            FilePowers = filename2;
            TouchedSun = false;
            LeftoverRibbons = 20;
            SlotMessage = "Empty";
            TutorialSteps = new List<int>();
            if (File.Exists("save/" + FilePowers))
            {
                using (StreamReader streamReader = new StreamReader("save/" + FilePowers))
                {
                    try
                    {
                        TouchedSun = bool.Parse(streamReader.ReadLine());
                        LeftoverRibbons = int.Parse(streamReader.ReadLine());
                    }
                    catch { }
                }
                Update();
            }
        }

        public void Update()
        {
            if (LeftoverRibbons == 20)
            {
                SlotMessage = "Just Started";
            }
            else if (TouchedSun)
            {
                SlotMessage = "Touched the Sun!";
            }
            else
            {
                SlotMessage = "Checkpoint #" + (20 - LeftoverRibbons);
            }
        }
    }
}
