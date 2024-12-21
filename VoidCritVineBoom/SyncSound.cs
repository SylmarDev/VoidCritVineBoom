using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using R2API.Networking.Interfaces;
using UnityEngine.Networking;
using UnityEngine;

namespace SylmarDev.VoidCritVineBoom
{
    public class SyncSound : INetMessage, ISerializableObject
    {
        public SyncSound() { }

        public SyncSound(UnityEngine.Vector3 soundPosition)
        {
            this.soundPosition = soundPosition;
        }
        public void Deserialize(NetworkReader reader)
        {
            this.soundPosition = reader.ReadVector3();
        }
        public void OnReceived()
        {
            if (!NetworkServer.active)
            {
                AkSoundEngine.PostEvent("Play_vineboom", new GameObject
                {
                    transform =
                    {
                        position = this.soundPosition
                    }
                });
            }
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.soundPosition);
        }

        private UnityEngine.Vector3 soundPosition;
    }
}
