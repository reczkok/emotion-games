using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class UnityEvents
    {
        public const string EndDialogueEvent = "dialogue_ended";
        public const string EndPhotoEvent = "end_photo_event";
        public const string CameraLock = "camera_lock";
        public const string CameraUnlock = "camera_unlock";
        public const string MovementLock = "movement_lock";
        public const string MovementUnlock = "movement_unlock";
        public const string PhotoDown = "photo_down";
    }
}
