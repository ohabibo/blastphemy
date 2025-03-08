using System;
using FMOD;
using Microsoft.Xna.Framework;

namespace Blok3Game.content.Scripts.Audio 
{
    public class FMODAudio 
    {
        private const string MASTER_BANK = "Assets/Audio/Music/Master.bank";
        private const string MASTER_STRINGS_BANK = "Assets/Audio/Music/Master.strings.bank";

        private FMOD.System system;
        private FMOD.Studio.System studioSystem;
        private FMOD.Studio.Bank masterBank;
        private FMOD.Studio.Bank stringsBank;
        private FMOD.Studio.EventDescription eventDescription;
        private FMOD.Studio.EventInstance eventInstance;

        public void Initialize()
        {
            FMOD.Factory.System_Create(out system);
            system.init(32, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);

            // FMOD.Studio.System.create(out studioSystem);
            // system.init(32, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);

            studioSystem.loadBankFile(MASTER_BANK, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out masterBank);
            studioSystem.loadBankFile(MASTER_STRINGS_BANK, FMOD.Studio.LOAD_BANK_FLAGS.NORMAL, out stringsBank);

            ListAllEvents();
        }

        private void ListAllEvents()
        {
            try
            {
                // Get list of all event descriptions in the loaded banks
                int count = 0;
                masterBank.getEventCount(out count);
                Console.WriteLine($"Events in master bank: {count}");
                
                FMOD.Studio.EventDescription[] events = new FMOD.Studio.EventDescription[count];
                masterBank.getEventList(out events);
                
                Console.WriteLine("Available FMOD events:");
                foreach (var evt in events)
                {
                    string path;
                    evt.getPath(out path);
                    Console.WriteLine($"  - {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing events: {ex.Message}");
            }
        }

        public void PlayMusic(string eventPath) 
        {
            // Get the event description
            FMOD.RESULT result = studioSystem.getEvent(eventPath, out eventDescription);
            
            Console.WriteLine($"Get event result: {result}");
            
            // Check if the event was found
            if (result == FMOD.RESULT.OK && eventDescription.isValid())
            {
                result = eventDescription.createInstance(out eventInstance);
                Console.WriteLine($"Create instance result: {result}");
                
                if (result == FMOD.RESULT.OK)
                {
                    // Start the event (actually plays the music)
                    result = eventInstance.start();
                    Console.WriteLine($"Start event result: {result}");
                }
            }
            else
            {
                Console.WriteLine($"Failed to find event: {eventPath}");
            }
        }

        public void Update() 
        {
            system.update();
            studioSystem.update();
        }

        public void Dispose()
        {
            eventInstance.release();
            masterBank.unload();
            stringsBank.unload();
            studioSystem.release();
            system.release();
        }
    }
}
